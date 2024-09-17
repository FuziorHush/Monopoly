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
    [SerializeField] private GameObject _balanceTextPrefab;
    [SerializeField] private Transform _balanceTextsParent;
    [SerializeField] private TMP_Text _playerTurnText;

    [SerializeField] private RectTransform _cardAnimation;
    [SerializeField] private float _cardAnimationOffset;
    [SerializeField] private float _cardAnimationTime;
    [SerializeField] private TMP_Text _cardText;

    private List<TMP_Text> _balanceTexts = new List<TMP_Text>();

    protected override void Awake()
    {
        _tossDices.onClick.AddListener(MakeTurn);
        _newTurn.onClick.AddListener(NewTurn);

        GameEvents.DicesTossed += OnDicesTossed;
        GameEvents.MoveAnimationStarted += OnAnimationStarted;
        GameEvents.MoveAnimationEnded += OnAnimationEnded;
        GameEvents.PlayerCreated += OnPlayerCreated;
        GameEvents.PlayerBalanceChanged += OnPlayerBalanceChanged;
        GameEvents.NewTurn += OnNewTurn;
        GameEvents.MatchStarted += OnMatchStarted;
        GameEvents.CardTriggered += OnCardTriggered;
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
        GameFlowController.Instance.NextTurn();
    }

    private void OnMatchStarted()
    {
        _playerTurnText.text = GameFlowController.Instance.PlayerWhoTurn.Name + " turn";
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
        if(GameFlowController.Instance.DicesActive)
        _tossDices.interactable = true;

        _newTurn.interactable = true;
    }

    private void OnPlayerCreated(Player player)
    {
        TMP_Text text = Instantiate(_balanceTextPrefab, _balanceTextsParent).GetComponent<TMP_Text>();
        text.text = player.Name + " " + player.Balance.ToString() + "$";
        _balanceTexts.Add(text);
    }

    private void OnPlayerBalanceChanged(Player player, float delta, float balance)
    {
        _balanceTexts[player.Number].text = player.Name + " " + balance.ToString() + "$";
    }

    private void OnNewTurn(Player player)
    {
        if (GameFlowController.Instance.DicesActive)
            _tossDices.interactable = true;

        _newTurn.interactable = true;

        if(EstateMenu.Instance.MenuIsOpen)
        EstateMenu.Instance.CloseMenu();

        _playerTurnText.text = player.Name + " turn";
    }

    private void OnCardTriggered(int cardID, CardDeck deck, Player player) {
        StartCoroutine("PlayCardAnimation");
        _cardText.text = cardID.ToString();
    }

    private IEnumerator PlayCardAnimation() 
    {
        GameEvents.MoveAnimationStarted?.Invoke();
        _cardAnimation.DOLocalMoveY(0, _cardAnimationTime);
        yield return new WaitForSeconds(_cardAnimationTime * 2);
        _cardAnimation.DOLocalMoveY(_cardAnimationOffset, _cardAnimationTime).OnComplete(delegate { _cardAnimation.anchoredPosition = new Vector3(0, -_cardAnimationOffset, 0); });
        GameEvents.MoveAnimationEnded?.Invoke();
    }
}
