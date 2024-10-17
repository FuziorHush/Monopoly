using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuckDeck : CardDeck
{
    private void Awake()
    {
        _allCardActions.Add(Action0);
        _allCardActions.Add(Action1);
        _allCardActions.Add(Action2);
        _allCardActions.Add(Action3);
        _allCardActions.Add(Action4);
        _allCardActions.Add(Action5);
        _allCardActions.Add(Action6);
        _allCardActions.Add(Action7);
        _allCardActions.Add(Action8);
        _allCardActions.Add(Action9);
        _allCardActions.Add(Action10);
        _allCardActions.Add(Action11);
        _allCardActions.Add(Action12);
        _allCardActions.Add(Action13);
        _allCardActions.Add(Action14);
        _allCardActions.Add(Action15);

        _currentCardActions = new List<CardAction>(_allCardActions);
    }

    private void Action0()
    {
        BalancesController.Instance.AddBalance(_targetPlayer, 200f);
    }

    private void Action1()
    {
        BalancesController.Instance.AddBalance(_targetPlayer, 25f);
    }

    private void Action2()
    {
        JailController.Instance.GiveGOJ(_targetPlayer);
    }

    private void Action3()
    {
        BalancesController.Instance.AddBalance(_targetPlayer, 25f);
    }

    private void Action4()
    {
        BalancesController.Instance.AddBalance(_targetPlayer, 100f);
    }

    private void Action5()
    {
        BalancesController.Instance.AddBalance(_targetPlayer, 25f);
    }

    private void Action6()
    {
        FieldController.Instance.GoOnCellByID(_targetPlayer, 9);//jail
        JailController.Instance.SendPlayerToJail(_targetPlayer, 3);
    }
    private void Action7()
    {
        BalancesController.Instance.WidthdrawBalance(_targetPlayer, 50f);
    }

    private void Action8()
    {
        BalancesController.Instance.AddBalance(_targetPlayer, 10f);
    }

    private void Action9()
    {
        BalancesController.Instance.AddBalance(_targetPlayer, 50f);
    }

    private void Action10()
    {
        BalancesController.Instance.WidthdrawBalance(_targetPlayer, 100f);
    }

    private void Action11()
    {
        FieldController.Instance.GoBackward(_targetPlayer, 5);
    }

    private void Action12()
    {
        BalancesController.Instance.AddBalance(_targetPlayer, 100f);
    }

    private void Action13()
    {
        float moneyGet = 0;
        for (int i = 0; i < GameFlowController.Instance.Players.Count; i++)
        {
            Player player = GameFlowController.Instance.Players[i];

            if (player == _targetPlayer)
                continue;

            if (player.Balance > 10) {
                moneyGet += 10;
                BalancesController.Instance.WidthdrawBalance(player, 10f);
            }
            else if (player.Balance > 0) {
                moneyGet += player.Balance;
                BalancesController.Instance.WidthdrawBalance(player, player.Balance);
            }

            if (moneyGet > 0) {
                BalancesController.Instance.AddBalance(_targetPlayer, moneyGet);
            }
        }
    }

    private void Action14()
    {
        BalancesController.Instance.WidthdrawBalance(_targetPlayer, 10f);//TODO: choose to pick chance
    }

    private void Action15()
    {
        BalancesController.Instance.WidthdrawBalance(_targetPlayer, 50f);
    }
}
