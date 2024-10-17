using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BankController : MonoSingleton<BankController>
{
    protected List<Player> _players;

    protected override void Awake()
    {
        base.Awake();
        GameEvents.ControllersCreated += Init;
    }

    private void Init()
    {
        _players = GameFlowController.Instance.Players;
    }
}
