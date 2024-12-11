using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class HUDController : MonoSingleton<HUDController>
{
    [SerializeField] private TMP_Text _dicesText;
    [SerializeField] private Button _tossDices;
    [SerializeField] private Button _newTurn;
    [SerializeField] private Button _getFree;
    [SerializeField] private Button _musicButton;
    [SerializeField] private GameObject _balanceTextPrefab;
    [SerializeField] private Transform _balanceTextsParent;
    [SerializeField] private TMP_Text _playerTurnText;

    [SerializeField] private RectTransform _cardAnimation;
    [SerializeField] private float _cardAnimationOffset;
    [SerializeField] private float _cardAnimationTime;
    [SerializeField] private float _cardWaitTime;
    [SerializeField] private TMP_Text _cardText;

    [SerializeField] private TMP_Text _endMessage;

    private List<TMP_Text> _balanceTexts = new List<TMP_Text>();
    private bool _negativeBalance;
    private IHUDAllowInput _allowInput;

    protected override void Awake()
    {
        _tossDices.onClick.AddListener(MakeTurn);
        _newTurn.onClick.AddListener(NewTurn);
        _getFree.onClick.AddListener(GetFree);
        _musicButton.onClick.AddListener(SwitchMusic);

        GameEvents.DicesTossed += OnDicesTossed;
        GameEvents.MoveAnimationStarted += OnAnimationStarted;
        GameEvents.MoveAnimationEnded += OnAnimationEnded;
        GameEvents.PlayerBalanceAdded += OnPlayerBalanceAdded;
        GameEvents.PlayerBalanceWidthdrawn += OnPlayerBalanceWidthdrawn;
        GameEvents.BalanceTransfered += OnPlayerBalanceTransfered;
        GameEvents.NewTurn += OnNewTurn;
        GameEvents.MatchStarted += OnMatchStarted;
        GameEvents.CardTriggered += OnCardTriggered;
        GameEvents.MatchEnded += OnMatchEnded;

        if (MultiplayerGameTypeController.CurrentType == MultiplayerGameType.Local) 
        {
            _allowInput = new HUDAllowInputLocal();
        } 
        else if (MultiplayerGameTypeController.CurrentType == MultiplayerGameType.Photon) 
        {
            _allowInput = new HUDAllowInputPhoton();
        }
    }

    private void Start()
    {
        _cardAnimation.anchoredPosition = new Vector3(0, -_cardAnimationOffset, 0);
    }

    private void MakeTurn()
    {
        GameFlowController.Instance.MakeTurn();
    }

    private void NewTurn() 
    {
        _newTurn.interactable = false;
        GameFlowController.Instance.NextTurn();
    }

    private void GetFree()
    {
        JailController.Instance.UseGOJ(GameFlowController.Instance.PlayerWhoTurn);
    }

    private void SwitchMusic() 
    {
        if (MusicController.Instance.MusicEnabled)
        {
            MusicController.Instance.DisableMusic();
        }
        else {
            MusicController.Instance.EnableMusic();
        }
    }

    private void OnMatchStarted()
    {
        _playerTurnText.text = $"<color=#{GameFlowController.Instance.PlayerWhoTurn.AvatarColorHex}>{GameFlowController.Instance.PlayerWhoTurn.Name}</color> turn";

        if (_allowInput.IsInputAllowed())
        {
            _tossDices.interactable = true;
            _newTurn.interactable = true;
        }
        else 
        {
            _tossDices.interactable = false;
            _newTurn.interactable = false;
        }

        for (int i = 0; i < GameFlowController.Instance.Players.Count; i++)
        {
            Player player = GameFlowController.Instance.Players[i];
            TMP_Text text = Instantiate(_balanceTextPrefab, _balanceTextsParent).GetComponent<TMP_Text>();
            text.text = $"<color=#{player.AvatarColorHex}>{player.Name}</color>({player.Team.Name}) { player.Balance}$";
            _balanceTexts.Add(text);
        }
    }

    private void OnDicesTossed(Vector2Int dicesValue)
    {
        _dicesText.text = $"{dicesValue.x} {dicesValue.y}";
    }

    private void OnAnimationStarted()
    {
        _tossDices.interactable = false;
        _newTurn.interactable = false;
    }

    private void OnAnimationEnded()
    {
        if (!_allowInput.IsInputAllowed())
            return;

        if (!_negativeBalance)
        {
            if (GameFlowController.Instance.DicesActive)
                _tossDices.interactable = true;

            _newTurn.interactable = true;
        }
    }

    private void OnPlayerBalanceAdded(Player player, float delta)
    {
        _balanceTexts[player.Number].text = $"<color=#{player.AvatarColorHex}>{player.Name}</color>({player.Team.Name}) { player.Balance}$";

        if (!_allowInput.IsInputAllowed())
            return;

        if (GameFlowController.Instance.PlayerWhoTurn == player)
        {
            if (_negativeBalance)
            {
                if (player.Balance >= 0)
                {
                    _negativeBalance = false;
                    if (GameFlowController.Instance.DicesActive)
                        _tossDices.interactable = true;

                    _newTurn.interactable = true;
                }
            }
        }
    }

    private void OnPlayerBalanceWidthdrawn(Player player, float delta)
    {
        _balanceTexts[player.Number].text = $"<color=#{player.AvatarColorHex}>{player.Name}</color>({player.Team.Name}) { player.Balance}$";

        if (!_allowInput.IsInputAllowed())
            return;

        if (GameFlowController.Instance.PlayerWhoTurn == player)
        {
            if (!_negativeBalance)
            {
                if (player.Balance < 0)
                {
                    _negativeBalance = true;
                    _tossDices.interactable = false;
                    _newTurn.interactable = false;
                }
            }
        }
    }

    private void OnPlayerBalanceTransfered(Player playerSender, Player playerReciever, float amount)
    {
        _balanceTexts[playerSender.Number].text = $"<color=#{playerSender.AvatarColorHex}>{playerSender.Name}</color>({playerSender.Team.Name}) { playerSender.Balance}$";

        _balanceTexts[playerReciever.Number].text = $"<color=#{playerReciever.AvatarColorHex}>{playerReciever.Name}</color>({playerReciever.Team.Name}) { playerReciever.Balance}$";

        if (!_allowInput.IsInputAllowed())
            return;

        if (GameFlowController.Instance.PlayerWhoTurn == playerSender)
        {
            if (!_negativeBalance)
            {
                if (playerSender.Balance < 0)
                {
                    _negativeBalance = true;
                    _tossDices.interactable = false;
                    _newTurn.interactable = false;
                }
            }
        }
    }

    private void OnNewTurn(Player player)
    {
        _playerTurnText.text = $"<color=#{player.AvatarColorHex}>{player.Name}</color> turn";

        if (!_allowInput.IsInputAllowed())
            return;

        if (GameFlowController.Instance.DicesActive)
            _tossDices.interactable = true;

        _newTurn.interactable = true;

        if (JailController.Instance.IsPlayerInJail(player))
        {
            _getFree.gameObject.SetActive(true);
            _getFree.interactable = player.GOJHas > 0;
        }
        else
        {
            _getFree.gameObject.SetActive(false);
        }
    }

    private void OnCardTriggered(int cardID, CardDeck deck, Player player) {
        if (deck is ChanceDeck)
        {
            _cardText.text = LanguageSystem.Instance[$"card_chance{cardID}"];
        }
        else if (deck is LuckDeck) 
        {
            _cardText.text = LanguageSystem.Instance[$"card_luck{cardID}"];
        }
        StartCoroutine("PlayCardAnimation");
    }

    private IEnumerator PlayCardAnimation() 
    {
        GameEvents.MoveAnimationStarted?.Invoke();
        _cardAnimation.DOLocalMoveY(0, _cardAnimationTime);
        yield return new WaitForSeconds(_cardAnimationTime + _cardWaitTime);
        _cardAnimation.DOLocalMoveY(_cardAnimationOffset, _cardAnimationTime).OnComplete(delegate { _cardAnimation.anchoredPosition = new Vector3(0, -_cardAnimationOffset, 0); });
        GameEvents.MoveAnimationEnded?.Invoke();
    }

    private void OnMatchEnded(Team teamWinner)
    {
        if (teamWinner == null)
        {
            _endMessage.text = "Tie";
        }
        else if (teamWinner.NumPlayers == 1)
        {
            _endMessage.text = teamWinner[0].Name + " wins";
        }
        else {
            _endMessage.text = teamWinner.Name + " wins";
        }
    }
}
