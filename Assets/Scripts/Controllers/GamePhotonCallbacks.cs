using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GamePhotonCallbacks : MonoBehaviourPunCallbacks
{
    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            GameFlowController.Instance.RemovePlayer(GameFlowController.Instance.Players.Find(x => x.NetworkPlayer.NickName == otherPlayer.NickName));
        }
        else if (otherPlayer.IsMasterClient) {
            PhotonNetwork.LeaveRoom();
        }
    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.SetPlayerCustomProperties(null);
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
}
