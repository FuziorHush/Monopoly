using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;

public class DecksControllerPhoton : DecksController, IOnEventCallback
{
    public override void TriggerChanceDeck(Player player)
    {
        int triggeredId = _chanceDeck.TriggerRandomAction(player);
        
    }

    public override void TriggerLuckDeck(Player player)
    {
        int triggeredId = _luckDeck.TriggerRandomAction(player);
    }

    private void TranslateActionTrigger_All(int cardActionIndex, CardDeck deck, Player player)
    {

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
        if (photonEvent.Code == (byte)PhotonEventCodes.DeckTriggered)
        {
            object[] data = (object[])photonEvent.CustomData;
            byte cardActionIndex = (byte)data[0];
            byte deck = (byte)data[1];//chance - 0, luck - 1
            Player player = (Player)data[2];
            //TranslateActionTrigger_All(cardActionIndex, player);
        }
    }
}
