using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

//!!!prototype!!!
public class RoomManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private Button _leaveButton;
    [SerializeField] private Transform _playerItemsParent;
    [SerializeField] private GameObject _playerItemPrefab;

    [SerializeField] private GameObject _colorItemPrefab;
    [SerializeField] private GameObject _iconItemPrefab;

    [SerializeField] private TMPro.TMP_InputField _startBalanceInput;
    [SerializeField] private Button _startButton;

    private List<PlayerInLobbyItem> _playerInLobbyItems = new List<PlayerInLobbyItem>();
    [SerializeField] private Transform _colorItemsParent;
    [SerializeField] private Transform _iconItemsParent;
    private List<GameObject> _colorItems = new List<GameObject>();
    private List<GameObject> _iconItems = new List<GameObject>();

    [SerializeField] private Sprite[] _sprites;
    private AvatarColor[] _avatarColors;

    [SerializeField] private Color _selectedColor;

    private PlayerInLobbyItem _myPlayerLobbyItem;
    private int _currentColorID;
    private int _currentIconID;

    ExitGames.Client.Photon.Hashtable _props = new ExitGames.Client.Photon.Hashtable();

    private void Awake()
    {
        _startButton.onClick.AddListener(StartGame);
        _leaveButton.onClick.AddListener(LeaveRoom);
        _avatarColors = Resources.LoadAll<AvatarColor>("AvatarColors");
        _startBalanceInput.text = GamePropertiesController.Instance.GameProperties.StartPlayerBalance.ToString();

        for (int i = 0; i < _avatarColors.Length; i++)
        {
            GameObject colorItem = Instantiate(_colorItemPrefab, _colorItemsParent).gameObject;
            int cl = i;
            colorItem.GetComponent<Button>().onClick.AddListener(delegate { SetPlayerColorID(cl); });
            colorItem.transform.GetChild(0).GetComponent<Image>().color = _avatarColors[i].FrontColor;
            _colorItems.Add(colorItem);
        }

        for (int i = 0; i < _sprites.Length; i++)
        {
            GameObject spriteItem = Instantiate(_iconItemPrefab, _iconItemsParent).gameObject;
            int cl = i;
            spriteItem.GetComponent<Button>().onClick.AddListener(delegate { SetPlayerSpriteID(cl); });
            spriteItem.GetComponent<Image>().sprite = _sprites[i];
            _iconItems.Add(spriteItem);
        }
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
        CreatePlayerCustomProperties();
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

    public void StartGame() 
    {
        float startBalance = GamePropertiesController.Instance.GameProperties.StartPlayerBalance;
        try
        {
            startBalance = int.Parse(_startBalanceInput.text);
        }
        catch (System.Exception)
        {

        }
        GamePropertiesController.Instance.GameProperties.StartPlayerBalance = startBalance;
        PhotonNetwork.LoadLevel(2);
    }

    /*
    public void SetPlayerColorInList(int id) 
    {
        _colorItems[_currentColorID].GetComponent<Image>().color = Color.clear;
        _colorItems[id].GetComponent<Image>().color = _selectedColor;
        
        _currentColorID = id;
    }

    public void SetPlayerSpriteInList(int id)
    {
        _iconItems[_currentColorID].transform.GetChild(0).GetComponent<Image>().color = Color.clear;
        _iconItems[id].transform.GetChild(0).GetComponent<Image>().color = _selectedColor;
        _currentIconID = id;
    }*/

    public void SetPlayerSpriteID(int id) 
    {
        _props["Icon"] = id;
        PhotonNetwork.SetPlayerCustomProperties(_props);
    }

    public void SetPlayerColorID(int id)
    {
        _props["Color"] = id;
        PhotonNetwork.SetPlayerCustomProperties(_props);
    }

    public void SetPlayerSpriteInList(PlayerInLobbyItem playerItem, int spriteID) 
    {
        playerItem.SetIcon(_sprites[spriteID]);
    }

    public void SetPlayerColorInList(PlayerInLobbyItem playerItem, int colorID)
    {
        playerItem.SetColor(_avatarColors[colorID]);
    }

    private void UpdatePlayerList()
    {
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
            playerItem.Init(player.Value.NickName, _avatarColors[0], _sprites[0]);
            _playerInLobbyItems.Add(playerItem);

            if (player.Value.NickName == PhotonNetwork.NickName) {
                _myPlayerLobbyItem = playerItem;
            }
        }
    }

    private void CreatePlayerCustomProperties() 
    {
        _props["Color"] = 0;
        _props["Icon"] = 0;
        PhotonNetwork.SetPlayerCustomProperties(_props);
    }

    public override void OnPlayerPropertiesUpdate(Photon.Realtime.Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);
        for (int i = 0; i < _playerInLobbyItems.Count; i++)
        {
            if (_playerInLobbyItems[i].Owner == targetPlayer.NickName) 
            {
                if (changedProps.ContainsKey("Color"))
                {
                    SetPlayerColorInList(_playerInLobbyItems[i], (int)changedProps["Color"]);
                }
                if (changedProps.ContainsKey("Icon")) {
                    SetPlayerSpriteInList(_playerInLobbyItems[i], (int)changedProps["Icon"]);
                }
            }
        }
    }
}