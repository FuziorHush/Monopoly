using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChanceDeck : CardDeck
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
        FieldController.Instance.GoOnCellByID(_targetPlayer, 27);//goto olymp
    }

    private void Action1()
    {
        FieldController.Instance.GoOnCellByID(_targetPlayer, 18);//goto bank
    }

    private void Action2()
    {
        float willPay = 0;
        for (int i = 0; i < _targetPlayer.EstatesOwn.Count; i++)
        {
            if (_targetPlayer.EstatesOwn[i].Level > 1 && _targetPlayer.EstatesOwn[i].Level < 5) {
                willPay += 25;
            }else if (_targetPlayer.EstatesOwn[i].Level == 5)
            {
                willPay += 100;
            }
        }

        if (willPay > 0) {
            BalancesController.Instance.WidthdrawBalance(_targetPlayer, willPay);
        }
    }

    private void Action3()
    {
        BalancesController.Instance.WidthdrawBalance(_targetPlayer, 150f);
    }

    private void Action4()
    {
        FieldController.Instance.GoOnCellByID(_targetPlayer, 0);
    }

    private void Action5()
    {
        BalancesController.Instance.WidthdrawBalance(_targetPlayer, 15f);
    }

    private void Action6()
    {
        FieldController.Instance.GoOnCellByID(_targetPlayer, 25);//tokyo
    }

    private void Action7()
    {
        BalancesController.Instance.AddBalance(_targetPlayer, 100f);
    }

    private void Action8()
    {
        JailController.Instance.GiveGOJ(_targetPlayer);
    }

    private void Action9()
    {
        BalancesController.Instance.AddBalance(_targetPlayer, 150f);
    }

    private void Action10()
    {
        BalancesController.Instance.AddBalance(_targetPlayer, 50f);
    }

    private void Action11()
    {
        FieldController.Instance.GoOnCellByID(_targetPlayer, 31);//london
    }

    private void Action12()
    {
        float willPay = 0;
        for (int i = 0; i < _targetPlayer.EstatesOwn.Count; i++)
        {
            if (_targetPlayer.EstatesOwn[i].Level > 1 && _targetPlayer.EstatesOwn[i].Level < 5)
            {
                willPay += 40;
            }
            else if (_targetPlayer.EstatesOwn[i].Level == 5)
            {
                willPay += 115;
            }
        }

        if (willPay > 0)
        {
            BalancesController.Instance.WidthdrawBalance(_targetPlayer, willPay);
        }
    }

    private void Action13()
    {
        FieldController.Instance.GoOnCellByID(_targetPlayer, 9);//jail
        JailController.Instance.SendPlayerToJail(_targetPlayer, 3);
    }

    private void Action14()
    {
        BalancesController.Instance.WidthdrawBalance(_targetPlayer, 20f);
    }

    private void Action15()
    {
        FieldController.Instance.GoBackward(_targetPlayer, 3);
    }
}
