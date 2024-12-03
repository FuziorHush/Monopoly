using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;

public class BalancesControllerPhoton : BalancesController, IOnEventCallback
{
    public override void AddBalance(Player player, float amount)
    {
        object[] data = new object[2];
        data[0] = (byte)_players.IndexOf(player);
        data[1] = amount;
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent((byte)PhotonEventCodes.PlayerBalanceAdded, data, raiseEventOptions, SendOptions.SendReliable);
    }

    private void AddBalance_All(Player player, float amount)
    {
        player.Balance += amount;
        GameEvents.PlayerBalanceAdded?.Invoke(player, amount);
    }

    public override void WidthdrawBalance(Player player, float amount)
    {
        object[] data = new object[2];
        data[0] = (byte)_players.IndexOf(player);
        data[1] = amount;
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent((byte)PhotonEventCodes.PlayerBalanceWidthdrawn, data, raiseEventOptions, SendOptions.SendReliable);
    }

    private void WidthdrawBalance_All(Player player, float amount)
    {
        player.Balance -= amount;
        GameEvents.PlayerBalanceWidthdrawn?.Invoke(player, amount);
    }

    public override void Transferbalance(Player playerSender, Player playerReceiver, float amount)
    {
        object[] data = new object[3];
        data[0] = (byte)_players.IndexOf(playerSender);
        data[1] = (byte)_players.IndexOf(playerReceiver);
        data[2] = amount;
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent((byte)PhotonEventCodes.BalanceTransfered, data, raiseEventOptions, SendOptions.SendReliable);
    }

    private void Transferbalance_All(Player playerSender, Player playerReceiver, float amount) 
    {
        playerSender.Balance -= amount;
        playerReceiver.Balance += amount;
        GameEvents.BalanceTransfered?.Invoke(playerSender, playerReceiver, amount);
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
        if (photonEvent.Code == (byte)PhotonEventCodes.PlayerBalanceAdded)
        {
            object[] data = (object[])photonEvent.CustomData;
            Player player = _players[(byte)data[0]];
            float amount = (float)data[1];
            AddBalance_All(player, amount);
        }
        else if (photonEvent.Code == (byte)PhotonEventCodes.PlayerBalanceWidthdrawn)
        {
            object[] data = (object[])photonEvent.CustomData;
            Player player = _players[(byte)data[0]];
            float amount = (float)data[1];
            WidthdrawBalance_All(player, amount);
        }
        else if (photonEvent.Code == (byte)PhotonEventCodes.BalanceTransfered)
        {
            object[] data = (object[])photonEvent.CustomData;
            Player playerSender = _players[(byte)data[0]];
            Player playerReceiver = _players[(byte)data[1]];
            float amount = (float)data[2];
            Transferbalance_All(playerSender, playerReceiver, amount);
        }
    }
}
