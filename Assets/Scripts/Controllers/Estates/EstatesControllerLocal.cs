using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EstatesControllerLocal : EstatesController
{
    public override void Purchase(int lvl)
    {
        if (_targetEstate.Owner == _targetPlayer)
        {
            UpgradeEstate(lvl);
        }
        else
        {
            BuyEstate(lvl);
        }
        _targetEstate.CellLink.UpdateBuildingSprite();
    }

    private void BuyEstate(int lvl)
    {
        _targetPlayer.Balance -= _prices[lvl - 1];
        _targetPlayer.EstatesOwn.Add(_targetEstate);
        _targetEstate.Owner = _targetPlayer;
        _targetEstate.CurrentQuantity = _prices[lvl - 1];
        _targetEstate.Level = lvl;

        GameEvents.PlayerBoughtEstate?.Invoke(_targetPlayer, _targetEstate);
    }

    private void UpgradeEstate(int lvl)
    {
        _targetPlayer.Balance -= _prices[lvl - 1];
        _targetEstate.CurrentQuantity += _prices[lvl - 1];
        _targetEstate.Level = lvl;

        GameEvents.PlayerUpgradedEstate?.Invoke(_targetPlayer, _targetEstate);
    }
}
