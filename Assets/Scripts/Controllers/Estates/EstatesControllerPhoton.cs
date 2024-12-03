using UnityEngine;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;

public class EstatesControllerPhoton : EstatesController, IOnEventCallback
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
    }

    public override void EstateCellClicked(Player player, Estate estate)
    {
        if (GameFlowController.Instance.PlayerWhoTurn != player && PhotonNetwork.LocalPlayer != player.NetworkPlayer)
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
        //target sets before
        float pledge = Mathf.RoundToInt(_targetEstate.CurrentQuantity / 2);
        BalancesController.Instance.AddBalance(_targetPlayer, pledge);

        object[] data = new object[3] {GameFlowController.Instance.Players.IndexOf(_targetPlayer),  (byte)Estates.IndexOf(_targetEstate), pledge};
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent((byte)PhotonEventCodes.EstatePledged, data, raiseEventOptions, SendOptions.SendReliable);
    }

    private void PledgeEstate_All(float pledgeAmount) 
    {
        _targetEstate.PledgedAmount = pledgeAmount;
        _targetEstate.CellLink.EnablePledgedEffect();
    }

    public override void UnpledgeTargetEstate()
    {
        //target sets before
        BalancesController.Instance.WidthdrawBalance(_targetPlayer, _targetEstate.PledgedAmount);

        object[] data = new object[2] { GameFlowController.Instance.Players.IndexOf(_targetPlayer), (byte)Estates.IndexOf(_targetEstate)};
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent((byte)PhotonEventCodes.EstateUnpledged, data, raiseEventOptions, SendOptions.SendReliable);
    }

    private void UnpledgeEstate_All() 
    {
        _targetEstate.PledgedAmount = 0;
        _targetEstate.CellLink.DisablePledgedEffect();
    }

    private void BuyEstate(int lvl) 
    {
        //target sets before
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };

        object[] data = new object[4];
        data[0] = (byte)_players.IndexOf(_targetPlayer);
        data[1] = (byte)Estates.IndexOf(_targetEstate);
        data[2] = (byte)lvl;
        data[3] = _prices[lvl - 1];
        PhotonNetwork.RaiseEvent((byte)PhotonEventCodes.PlayerBoughtEstate, data, raiseEventOptions, SendOptions.SendReliable);

        data = new object[2];
        data[0] = (byte)_players.IndexOf(_targetPlayer); ;
        data[1] = _prices[lvl - 1];
        PhotonNetwork.RaiseEvent((byte)PhotonEventCodes.PlayerBalanceWidthdrawn, data, raiseEventOptions, SendOptions.SendReliable);
    }

    private void UpgradeEstate(int lvl) 
    {
        //target sets before
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };

        object[] data = new object[4];
        data[0] = (byte)_players.IndexOf(_targetPlayer);
        data[1] = (byte)Estates.IndexOf(_targetEstate);
        data[2] = (byte)lvl;
        data[3] = _prices[lvl - 1];
        PhotonNetwork.RaiseEvent((byte)PhotonEventCodes.PlayerUpgradedEstate, data, raiseEventOptions, SendOptions.SendReliable);

        data = new object[2];
        data[0] = (byte)_players.IndexOf(_targetPlayer);
        data[1] = _prices[lvl - 1];
        PhotonNetwork.RaiseEvent((byte)PhotonEventCodes.PlayerBalanceWidthdrawn, data, raiseEventOptions, SendOptions.SendReliable);
    }

    private void BuyEstate_All(int lvl, float addedQuantity)
    {
        //target sets before
        _targetPlayer.EstatesOwn.Add(_targetEstate);
        _targetEstate.Owner = _targetPlayer;
        _targetEstate.CurrentQuantity = addedQuantity;
        _targetEstate.Level = lvl;

        if (_targetPlayer.NetworkPlayer == PhotonNetwork.LocalPlayer)
            EstateMenu.Instance.CloseMenu();

        _targetEstate.CellLink.UpdateBuildingSprite();

        GameEvents.PlayerBoughtEstate?.Invoke(_targetPlayer, _targetEstate);
    }

    private void UpgradeEstate_All(int lvl, float addedQuantity)
    {
        //target sets before
        _targetEstate.CurrentQuantity += addedQuantity;
        _targetEstate.Level = lvl;

        if (_targetPlayer.NetworkPlayer == PhotonNetwork.LocalPlayer)
            EstateMenu.Instance.CloseMenu();

        _targetEstate.CellLink.UpdateBuildingSprite();

        GameEvents.PlayerUpgradedEstate?.Invoke(_targetPlayer, _targetEstate);
    }

    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code == (byte)PhotonEventCodes.PlayerBoughtEstate)
        {
            object[] data = (object[])photonEvent.CustomData;
            Player player = _players[(byte)data[0]];
            Estate estate = Estates[(byte)data[1]];
            byte lvl = (byte)data[2];
            float addedQuantity = (float)data[3];

            SetTarget(player, estate);
            BuyEstate_All(lvl, addedQuantity);
        }
        else if (photonEvent.Code == (byte)PhotonEventCodes.PlayerUpgradedEstate)
        {
            object[] data = (object[])photonEvent.CustomData;
            Player player = _players[(byte)data[0]];
            Estate estate = Estates[(byte)data[1]];
            byte lvl = (byte)data[2];
            float addedQuantity = (float)data[3];

            SetTarget(player, estate);
            UpgradeEstate_All(lvl, addedQuantity);
        }
        else if (photonEvent.Code == (byte)PhotonEventCodes.EstatePledged)
        {
            object[] data = (object[])photonEvent.CustomData;
            Player player = _players[(byte)data[0]];
            Estate estate = Estates[(byte)data[1]];
            float pledgeAmount = (float)data[2];

            SetTarget(player, estate);
            PledgeEstate_All(pledgeAmount);
        }
        else if (photonEvent.Code == (byte)PhotonEventCodes.EstateUnpledged)
        {
            object[] data = (object[])photonEvent.CustomData;
            Player player = _players[(byte)data[0]];
            Estate estate = Estates[(byte)data[1]];

            SetTarget(player, estate);
            UnpledgeEstate_All();
        }
    }
}
