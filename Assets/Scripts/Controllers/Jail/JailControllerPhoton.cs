using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;

public class JailControllerPhoton : JailController, IOnEventCallback
{
    public override void SendPlayerToJail(Player player)
    {
        object[] data = new object[1] { (byte)_players.IndexOf(player)};
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent((byte)PhotonEventCodes.PlayerSentToJail, data, raiseEventOptions, SendOptions.SendReliable);
    }

    private void SendPlayerToJail_All(Player player)
    {
        _jailedPlayers.Add(new JailedPlayer(player, _jailTurns));
        GameEvents.PlayerSentToJail?.Invoke(player);
    }

    public override void TurnJail(Player player)//TODO: make void
    {
        object[] data = new object[1] { (byte)_players.IndexOf(player) };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent((byte)PhotonEventCodes.CheckJail, data, raiseEventOptions, SendOptions.SendReliable);
    }

    private void CheckJail_All(Player player)
    {
        JailedPlayer jailedPlayer = _jailedPlayers.Find(x => x.Player == player);
        if (jailedPlayer != null)
        {
            jailedPlayer.TurnsLeft--;
            if (jailedPlayer.TurnsLeft == 0)
            {
                _jailedPlayers.Remove(jailedPlayer);
                GameEvents.PlayerFreedFromJail?.Invoke(jailedPlayer.Player);
            }
        }
    }

    public override void GiveGOJ(Player player)
    {
        object[] data = new object[1] { (byte)_players.IndexOf(player) };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent((byte)PhotonEventCodes.PlayerGotGOJ, data, raiseEventOptions, SendOptions.SendReliable);
    }

    private void GiveGOJ_All(Player player) 
    {
        player.GOJHas++;
    }

    public override void UseGOJ(Player player)
    {
        object[] data = new object[1] { (byte)_players.IndexOf(player) };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent((byte)PhotonEventCodes.PlayerUsedGOJ, data, raiseEventOptions, SendOptions.SendReliable);
    }

    private void UseGOJ_All(Player player)
    {
        JailedPlayer jailedPlayer = _jailedPlayers.Find(x => x.Player == player);
        player.GOJHas--;
        _jailedPlayers.Remove(jailedPlayer);
        GameEvents.PlayerFreedFromJail?.Invoke(jailedPlayer.Player);
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
        if (photonEvent.Code == (byte)PhotonEventCodes.PlayerSentToJail)
        {
            object[] data = (object[])photonEvent.CustomData;
            Player player = _players[(byte)data[0]];
            SendPlayerToJail_All(player);
        }
        else if (photonEvent.Code == (byte)PhotonEventCodes.CheckJail) 
        {
            object[] data = (object[])photonEvent.CustomData;
            Player player = _players[(byte)data[0]];
            CheckJail_All(player);
        }
        else if (photonEvent.Code == (byte)PhotonEventCodes.PlayerGotGOJ)
        {
            object[] data = (object[])photonEvent.CustomData;
            Player player = _players[(byte)data[0]];
            GiveGOJ_All(player);
        }
        else if (photonEvent.Code == (byte)PhotonEventCodes.PlayerUsedGOJ)
        {
            object[] data = (object[])photonEvent.CustomData;
            Player player = _players[(byte)data[0]];
            UseGOJ_All(player);
        }
    }
}
