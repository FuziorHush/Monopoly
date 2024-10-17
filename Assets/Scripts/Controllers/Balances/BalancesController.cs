using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BalancesController : MonoSingleton<BalancesController>
{
    protected List<Player> _players;

    protected override void Awake()
    {
        base.Awake();
        GameEvents.ControllersCreated += Init;
    }

    private void Init() {
        _players = GameFlowController.Instance.Players;
    }

    public abstract void AddBalance(Player player, float amount);
    public abstract void WidthdrawBalance(Player player, float amount);
    public abstract void Transferbalance(Player playerSender, Player playerReceiver, float amount);
}
