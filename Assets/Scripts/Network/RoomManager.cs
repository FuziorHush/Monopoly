using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class RoomManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private Button _leaveButton;
    [SerializeField] private Transform _playerItemsParent;
    [SerializeField] private GameObject _playerItemPrefab;

    private List<PlayerInLobbyItem> _playerInLobbyItems = new List<PlayerInLobbyItem>();

    private void Awake()
    {
        _leaveButton.onClick.AddListener(LeaveRoom);
    }

    private void LeaveRoom() 
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        LobbyManager.Instance.SwitchToBrowseMenu();
    }

    public override void OnJoinedRoom()
    {
        UpdatePlayerList();
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        UpdatePlayerList();
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player newPlayer)
    {
        UpdatePlayerList();
    }

    private void UpdatePlayerList() {
        for (int i = 0; i < _playerInLobbyItems.Count; i++)
        {
            Destroy(_playerInLobbyItems[i].gameObject);
        }

        if (PhotonNetwork.CurrentRoom == null)
        {
            return;
        }

        foreach (KeyValuePair <int, Photon.Realtime.Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            PlayerInLobbyItem playerItem = Instantiate(_playerItemPrefab, _playerItemsParent).GetComponent<PlayerInLobbyItem>();
            _playerInLobbyItems.Add(playerItem);
        }
    }
}
