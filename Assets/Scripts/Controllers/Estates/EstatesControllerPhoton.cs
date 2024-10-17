using System.Collections;
using System.Collections.Generic;
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
        _targetEstate.CellLink.UpdateBuildingSprite();
    }

    private void BuyEstate(int lvl) 
    {
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };

        object[] data = new object[3] { _targetPlayer, _targetEstate, lvl };
        PhotonNetwork.RaiseEvent((byte)PhotonEventCodes.PlayerBoughtEstate, data, raiseEventOptions, SendOptions.SendReliable);

        data = new object[2] { _targetPlayer, _prices[lvl - 1] };
        PhotonNetwork.RaiseEvent((byte)PhotonEventCodes.PlayerBalanceWidthdrawn, data, raiseEventOptions, SendOptions.SendReliable);
    }

    private void UpgradeEstate(int lvl) 
    {
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };

        object[] data = new object[3] { _targetPlayer, _targetEstate, lvl };
        PhotonNetwork.RaiseEvent((byte)PhotonEventCodes.PlayerUpgradedEstate, data, raiseEventOptions, SendOptions.SendReliable);

        data = new object[2] { _targetPlayer, _prices[lvl - 1] };
        PhotonNetwork.RaiseEvent((byte)PhotonEventCodes.PlayerBalanceWidthdrawn, data, raiseEventOptions, SendOptions.SendReliable);
    }

    private void BuyEstate_All(Player player, Estate estate, int lvl)
    {
        player.EstatesOwn.Add(estate);
        estate.Owner = player;
        estate.CurrentQuantity = _prices[lvl - 1];// !!!
        estate.Level = lvl;

        GameEvents.PlayerBoughtEstate?.Invoke(player, estate);
    }

    private void UpgradeEstate_All(Player player, Estate estate, int lvl)
    {
        estate.CurrentQuantity += _prices[lvl - 1];//
        estate.Level = lvl;

        GameEvents.PlayerUpgradedEstate?.Invoke(player, estate);
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
            Player player = (Player)data[0];
            Estate estate = (Estate)data[1];
            int lvl = (int)data[2];
            BuyEstate_All(player, estate, lvl);
        }
        else if (photonEvent.Code == (byte)PhotonEventCodes.PlayerUpgradedEstate)
        {
            object[] data = (object[])photonEvent.CustomData;
            Player player = (Player)data[0];
            Estate estate = (Estate)data[1];
            int lvl = (int)data[2];
            UpgradeEstate_All(player, estate, lvl);
        }
    }
}
