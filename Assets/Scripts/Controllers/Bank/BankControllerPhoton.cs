using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;

public class BankControllerPhoton : BankController, IOnEventCallback
{
    public override void HandOverMoney(Player playerSender, Player playerReciever, float amount)
    {
        if (playerSender.Balance >= amount)
        {
            BalancesController.Instance.Transferbalance(playerSender, playerReciever, amount);
        }
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

    }
}
