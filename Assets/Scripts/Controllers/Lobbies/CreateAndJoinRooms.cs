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
    [SerializeField] private TMP_Text _versionText;

    [SerializeField] private Button _localGame;

    [SerializeField] private float _timeBetweenUpdates;

    private List<RoomInfo> _cachedRoomsList = new List<RoomInfo>();
    private float _nextUpdateTime;

    private void Awake()
    {
        Instance = this;

        _createButton.onClick.AddListener(CreateRoom);
        _localGame.onClick.AddListener(LocalGame);
        MultiplayerGameTypeController.CurrentType = MultiplayerGameType.NotStarted;
    }

    private void Start()
    {
        PhotonNetwork.JoinLobby();
        _versionText.text = Settings.GAME_VERSION;
    }

    private void Update()
    {
        if (PhotonNetwork.InRoom)
            return;

        if (Time.time < _nextUpdateTime)
            return;

        UpdateUI();
        _nextUpdateTime = Time.time + _timeBetweenUpdates;
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        print("ss");

        if (PhotonNetwork.InRoom)
            return;

        _cachedRoomsList = roomList;
        UpdateUI();
    }

    private void UpdateUI() 
    {
        foreach (Transform roomItem in _roomItemsParent)
        {
            Destroy(roomItem.gameObject);
        }

        foreach (var room in _cachedRoomsList)
        {
            GameObject roomItem = Instantiate(_roomItemPrefab, _roomItemsParent);
            roomItem.GetComponent<RoomlistItem>().Init(room.Name, room.PlayerCount);
        }
    }

    public void CreateRoom() 
    {
        PhotonNetwork.NickName = _nameInput.text;

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;
        roomOptions.BroadcastPropsChangeToAll = true;

        PhotonNetwork.CreateRoom(_createInput.text, roomOptions);
    }

    public void JoinRoom(string roomName) 
    {
        if (string.IsNullOrWhiteSpace(_nameInput.text))
            return;

        PhotonNetwork.NickName = _nameInput.text;
        PhotonNetwork.JoinRoom(roomName);
    }

    public override void OnJoinedRoom()
    {
        LobbyManager.Instance.SwitchToRoomMenu();
        MultiplayerGameTypeController.CurrentType = MultiplayerGameType.Photon;
    }

    private void LocalGame()
    {
        LobbyManager.Instance.SwitchToLocalMenu();
        MultiplayerGameTypeController.CurrentType = MultiplayerGameType.Local;
    }
}
