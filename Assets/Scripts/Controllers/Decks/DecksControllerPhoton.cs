using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;

public class DecksControllerPhoton : DecksController, IOnEventCallback
{
    public override void TriggerChanceDeck(Player player)
    {
        int triggeredId = _chanceDeck.TriggerRandomAction(player);

        object[] data = new object[3];
        data[0] = (byte)triggeredId;
        data[1] = (byte)0;
        data[2] = (byte)_players.IndexOf(player);
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent((byte)PhotonEventCodes.DeckTriggered, data, raiseEventOptions, SendOptions.SendReliable);
    }

    public override void TriggerLuckDeck(Player player)
    {
        int triggeredId = _luckDeck.TriggerRandomAction(player);

        object[] data = new object[3];
        data[0] = (byte)triggeredId;
        data[1] = (byte)1;
        data[2] = (byte)_players.IndexOf(player);
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent((byte)PhotonEventCodes.DeckTriggered, data, raiseEventOptions, SendOptions.SendReliable);
    }

    private void TranslateActionTrigger_All(int cardActionIndex, byte deck, Player player)
    {
        if (deck == 0)
        {
            _chanceDeck.TranslateActionTrigger(cardActionIndex, player);
        }
        else if (deck == 1) {
            _luckDeck.TranslateActionTrigger(cardActionIndex, player);
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
        if (photonEvent.Code == (byte)PhotonEventCodes.DeckTriggered)
        {
            object[] data = (object[])photonEvent.CustomData;
            byte cardActionIndex = (byte)data[0];
            byte deck = (byte)data[1];//chance - 0, luck - 1
            Player player = _players[(byte)data[2]];
            TranslateActionTrigger_All(cardActionIndex, deck, player);
        }
    }
}
