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
        EstateMenu.Instance.CloseMenu();
    }

    public override void EstateCellClicked(Player player, Estate estate)
    {
        if (GameFlowController.Instance.PlayerWhoTurn != player)
            return;

        if (GameFlowController.Instance.PlayerWhoTurn.Balance < 0 && estate.Owner == player && estate.PledgedAmount == 0)
        {
            SetTarget(player, estate);
            PledgeTargetEstate();
        }
        else if (OlympController.Instance.CanApplyOlympBonus && estate.Owner == player)
        {
            OlympController.Instance.SetBonusToEstate(estate);
        }
        else if (estate.PledgedAmount > 0 && estate.Owner == player)
        {
            if (player.Balance >= estate.PledgedAmount)
            {
                SetTarget(player, estate);
                UnpledgeTargetEstate();
            }
        }
    }

    public override void PledgeTargetEstate() 
    {
        float pledge = Mathf.RoundToInt(_targetEstate.CurrentQuantity / 2);
        _targetEstate.PledgedAmount = pledge;
        BalancesController.Instance.AddBalance(_targetPlayer, pledge);
        _targetEstate.CellLink.EnablePledgedEffect();
    }

    public override void UnpledgeTargetEstate()
    {
        BalancesController.Instance.WidthdrawBalance(_targetPlayer, _targetEstate.PledgedAmount);
        _targetEstate.PledgedAmount = 0;
        _targetEstate.CellLink.DisablePledgedEffect();
    }

    private void BuyEstate(int lvl)
    {
        BalancesController.Instance.WidthdrawBalance(_targetPlayer, _prices[lvl - 1]);
        _targetPlayer.EstatesOwn.Add(_targetEstate);
        _targetEstate.Owner = _targetPlayer;
        _targetEstate.CurrentQuantity = _prices[lvl - 1];
        _targetEstate.Level = lvl;

        GameEvents.PlayerBoughtEstate?.Invoke(_targetPlayer, _targetEstate);
    }

    private void UpgradeEstate(int lvl)
    {
        BalancesController.Instance.WidthdrawBalance(_targetPlayer, _prices[lvl - 1]);
        _targetEstate.CurrentQuantity += _prices[lvl - 1];
        _targetEstate.Level = lvl;

        GameEvents.PlayerUpgradedEstate?.Invoke(_targetPlayer, _targetEstate);
    }
}
