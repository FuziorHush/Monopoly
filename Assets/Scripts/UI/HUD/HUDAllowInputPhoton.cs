using Photon.Pun;
using UnityEngine;

public class HUDAllowInputPhoton : IHUDAllowInput
{
    public bool IsInputAllowed()
    {
        return GameFlowController.Instance.PlayerWhoTurn.NetworkPlayer.NickName == PhotonNetwork.LocalPlayer.NickName;
    }
}
