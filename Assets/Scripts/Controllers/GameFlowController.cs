using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameFlowController : MonoSingleton<GameFlowController>
{
    public List<Player> Players { get; private set; } = new List<Player>();
    public List<Team> Teams { get; private set; } = new List<Team>();

    public Player PlayerWhoTurn { get; private set; }
    private int _playerWhoTurnNum;
    private int _diceDoublesInTurn;

    public bool DicesActive { get; private set; }
    //private Dictionary<string, bool> _currentAvilableActions;

    public bool CurrentPlayerCanUseTrain { get; set; }
    public bool DontInteractWithNextCell { get; set; }

    protected override void Awake()
    {
        base.Awake();
        LanguageSystem.Instance.LoadLanguage("ru_RU");
    }

    public void Start()
    {
        GamePropertiesController.Instance.Init();
        FieldController.Instance.Init();
        EstateMenu.Instance.Init();

        GameEvents.PlayerBalanceIsNegative += OnPlayerBalanceIsNegative;

        StartMatch();
    }

    public void StartMatch()
    {
        for (int i = 0; i < 4; i++)
        {
            CreatePlayer($"player{i + 1}", i).Balance = 500;
        }

        Teams.Add(new Team("team1"));
        Teams[0].AddPlayer(Players[0]);
        Teams[0].AddPlayer(Players[1]);

        Teams.Add(new Team("team2"));
        Teams[0].AddPlayer(Players[2]);
        Teams[0].AddPlayer(Players[3]);

        //_currentAvilableActions = new Dictionary<string, bool>();
        //_currentAvilableActions.Add("dice", false);
        //_currentAvilableActions.Add("estate", false);
        //_currentAvilableActions.Add("goj", false);

        InitFirstTurn();

        GameEvents.MatchStarted?.Invoke();
    }

    /*
    public void SetTurnAction(string name, bool state)
    {
        _currentAvilableActions[name] = state;
        if (AllTurnActionsInactive())
            NextTurn();
    }

    public bool GetTurnActionState(string name) {
        return _currentAvilableActions[name];
    }*/

    public void MakeTurn()
    {
        CurrentPlayerCanUseTrain = false;

        Vector2Int dicesValue = TossDices();
        DicesActive = false;
        if (dicesValue.x == dicesValue.y)
        {
            if (_diceDoublesInTurn == 3)
            {
                JailController.Instance.SendPlayerToJail(PlayerWhoTurn, 3);
                _diceDoublesInTurn = 0;
                NextTurn();
                return;
            }
            _diceDoublesInTurn++;
            DicesActive = true;
        }
        else {
            _diceDoublesInTurn = 0;
        }
        FieldController.Instance.GoForward(PlayerWhoTurn, dicesValue.x + dicesValue.y);
    }

    public Vector2Int TossDices()
    {
        Vector2Int dicesValue = new Vector2Int(Random.Range(1, 7), Random.Range(1, 7));
        GameEvents.DicesTossed?.Invoke(dicesValue);
        return dicesValue;
    }

    /*
    private bool AllTurnActionsInactive() {
        foreach (KeyValuePair<string, bool> entry in _currentAvilableActions)
        {
            if (entry.Value)
                return false;
        }
        return true;
    }*/

    private void InitFirstTurn()
    {
        PlayerWhoTurn = Players[0];
        _playerWhoTurnNum = 0;
        // _currentAvilableActions["dice"] = true;
    }

    public void NextTurn()
    {
        CurrentPlayerCanUseTrain = false;

        _playerWhoTurnNum++;
        if (_playerWhoTurnNum == Players.Count) {
            _playerWhoTurnNum = 0;
        }
        PlayerWhoTurn = Players[_playerWhoTurnNum];

        DicesActive = !JailController.Instance.CheckJail(PlayerWhoTurn);

        GameEvents.NewTurn?.Invoke(PlayerWhoTurn);
    }

    private Player CreatePlayer(string name, int number)
    {
        Player player = new Player(name, number, FieldController.Instance.CreatePlayerAvatar(number));
        Players.Add(player);
        GameEvents.PlayerCreated?.Invoke(player);
        return player;
    }

    private void OnPlayerBalanceIsNegative(Player player, float balance) 
    {
        CheckIfPlayerCanPledge(player, balance);
    }

    private void CheckIfPlayerCanPledge(Player player, float balance) 
    {
        for (int i = 0; i < player.EstatesOwn.Count; i++)
        {
            balance += Mathf.RoundToInt((player.EstatesOwn[i].CurrentQuantity) / 2);
            if (balance >= 0) {
                return;
            }
        }

        RemovePlayer(player);
        NextTurn();
    }

    private void RemovePlayer(Player player)
    {
        for (int i = 0; i < player.EstatesOwn.Count; i++)
        {
            player.EstatesOwn[i].ResetEstate();
        }
        Players.Remove(player);
        Destroy(player.AvatarTransform.gameObject);
    }
}
