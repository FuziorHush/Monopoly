using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Realtime;
using ExitGames.Client.Photon;
using Photon.Pun;

public abstract class GameFlowController : MonoSingleton<GameFlowController>
{
    public List<Player> Players { get; private set; } = new List<Player>();
    public List<Team> Teams { get; private set; } = new List<Team>();

    public Player ControllerOwner;
    public Player PlayerWhoTurn { get; set; }
    public int _playerWhoTurnNum { get; protected set; }
    public int _diceDoublesInTurn { get; protected set; }

    public bool DicesActive { get;  set; }

    public bool CurrentPlayerCanUseTrain { get; set; }
    public bool DontInteractWithNextCell { get; set; }

    protected override void Awake()
    {
        base.Awake();

        GameEvents.PlayerBalanceIsNegative += OnPlayerBalanceIsNegative;
    }

    public abstract void StartMatch();
    public abstract void CreatePlayers();
    public abstract void MakeTurn();
    public abstract void NextTurn();
    protected abstract void RemovePlayer(Player player);

    private void OnPlayerBalanceIsNegative(Player player, float balance) 
    {
        CheckIfPlayerCanPledge(player, balance);
    }

    protected abstract void CheckIfPlayerCanPledge(Player player, float balance);

}
