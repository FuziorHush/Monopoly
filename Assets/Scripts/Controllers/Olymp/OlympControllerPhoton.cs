using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;

public class OlympControllerPhoton : OlympController, IOnEventCallback
{
    public override void SetBonusToEstate(Estate estate)
    {
        object[] data = new object[1] { estate };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent((byte)PhotonEventCodes.EstateBonusApplied, data, raiseEventOptions, SendOptions.SendReliable);
    }

    private void SetBonusToEstate_All(Estate estate)
    {
        if (_estateWithBonusApplied != null)
        {
            _estateWithBonusApplied.OlympBonusApplied = false;
        }
        _estateWithBonusApplied = estate;
        estate.OlympBonusApplied = true;
        CurrentOlympBonus *= _olympBonusMuiltiply;
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
        if (photonEvent.Code == (byte)PhotonEventCodes.EstateBonusApplied)
        {
            object[] data = (object[])photonEvent.CustomData;
            Estate estate = (Estate)data[0];
            SetBonusToEstate_All(estate);
        }
    }
}