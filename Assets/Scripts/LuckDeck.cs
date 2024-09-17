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
        _targetPlayer.Balance += 200;
    }

    private void Action1()
    {
        _targetPlayer.Balance += 25;
    }

    private void Action2()
    {
        _targetPlayer.GOJHas++;
    }

    private void Action3()
    {
        _targetPlayer.Balance += 25;
    }

    private void Action4()
    {
        _targetPlayer.Balance += 100;
    }

    private void Action5()
    {
        _targetPlayer.Balance += 25;
    }

    private void Action6()
    {
        FieldController.Instance.GoOnCellByID(_targetPlayer, 9);//jail
        JailController.Instance.SendPlayerToJail(_targetPlayer, 3);
    }
    private void Action7()
    {
        _targetPlayer.Balance -= 50;
    }

    private void Action8()
    {
        _targetPlayer.Balance += 10;
    }

    private void Action9()
    {
        _targetPlayer.Balance += 50;
    }

    private void Action10()
    {
        _targetPlayer.Balance -= 100;
    }

    private void Action11()
    {
        FieldController.Instance.GoBackward(_targetPlayer, 5);
    }

    private void Action12()
    {
        _targetPlayer.Balance += 100;
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
                player.Balance -= 10;
            }
            else if (player.Balance > 0) {
                moneyGet += player.Balance;
                player.Balance = 0;
            }

            if (moneyGet > 0) {
                _targetPlayer.Balance += moneyGet;
            }
        }
    }

    private void Action14()
    {
        _targetPlayer.Balance -= 10;//TODO: choose to pick chance
    }

    private void Action15()
    {
        _targetPlayer.Balance -= 50;
    }
}
