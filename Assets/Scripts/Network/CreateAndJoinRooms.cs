using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;
using Photon.Realtime;

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{
    public static CreateAndJoinRooms Instance;

    [SerializeField] private GameObject _roomItemPrefab;
    [SerializeField] private Transform _roomItemsParent;

    [SerializeField] private Button _createButton;
    [SerializeField] private TMP_InputField _createInput;
    [SerializeField] private TMP_InputField _nameInput;

    [SerializeField] private float _timeBetweenUpdates;

    private List<RoomInfo> _cachedRoomsList = new List<RoomInfo>();
    private float _nextUpdateTime;

    private void Awake()
    {
        Instance = this;

        _createButton.onClick.AddListener(CreateRoom);
    }

    /*
    private IEnumerator Start()
    {
        if (PhotonNetwork.InRoom) {
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.Disconnect();
        }

        yield return new WaitUntil(() => !PhotonNetwork.IsConnected);

        PhotonNetwork.ConnectUsingSettings();
    }*/

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        UpdateUI();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        if (Time.time < _nextUpdateTime)
            return;

        UpdateUI();
        _nextUpdateTime = Time.time + _timeBetweenUpdates;
    }

    private void UpdateUI() {
        foreach (Transform roomItem in _roomItemsParent)
        {
            Destroy(roomItem.gameObject);
        }

        foreach (var room in _cachedRoomsList)
        {
            GameObject roomItem = Instantiate(_roomItemPrefab, _roomItemsParent);
            roomItem.transform.GetChild(0).GetComponent<TMP_Text>().text = room.Name;
            roomItem.transform.GetChild(1).GetComponent<TMP_Text>().text = room.PlayerCount + "/4";
            roomItem.GetComponent<RoomlistItem>().RoomName = room.Name;
        }
    }

    public void CreateRoom() {
        PhotonNetwork.NickName = _nameInput.text;

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;

        PhotonNetwork.CreateRoom(_createInput.text, roomOptions);
    }

    public void JoinRoom(string roomName) 
    {
        PhotonNetwork.NickName = _nameInput.text;

        PhotonNetwork.JoinRoom(roomName);
    }

    public override void OnJoinedRoom()
    {
        LobbyManager.Instance.SwitchToRoomMenu();
    }
}
