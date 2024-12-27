using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

//prototype
public class RoomManagerPhoton : MonoBehaviourPunCallbacks
{
    [SerializeField] private Button _leaveButton;
    [SerializeField] private Transform _playerItemsParent;
    [SerializeField] private GameObject _playerItemPrefab;

    [SerializeField] private GameObject _colorItemPrefab;
    [SerializeField] private GameObject _iconItemPrefab;

    [SerializeField] private TMPro.TMP_InputField _startBalanceInput;
    [SerializeField] private Button _startButton;

    private List<PlayerInLobbyItemPhoton> _playerInLobbyItems = new List<PlayerInLobbyItemPhoton>();
    [SerializeField] private Transform _colorItemsParent;
    [SerializeField] private Transform _iconItemsParent;
    private List<GameObject> _colorItems = new List<GameObject>();
    private List<GameObject> _iconItems = new List<GameObject>();

    [SerializeField] private Color _selectedColor;

    ExitGames.Client.Photon.Hashtable _props = new ExitGames.Client.Photon.Hashtable();

    private void Awake()
    {
        _leaveButton.onClick.AddListener(LeaveRoom);
        _startBalanceInput.text = GamePropertiesController.GameProperties.StartPlayerBalance.ToString();

        AvatarColor[] avatarColors = StaticData.Instance.AvatarColors;
        for (int i = 0; i < avatarColors.Length; i++)
        {
            GameObject colorItem = Instantiate(_colorItemPrefab, _colorItemsParent).gameObject;
            int cl = i;
            colorItem.GetComponent<Button>().onClick.AddListener(delegate { SetPlayerColorID(cl); });
            colorItem.transform.GetChild(0).GetComponent<Image>().color = avatarColors[i].FrontColor;
            _colorItems.Add(colorItem);
        }

        Sprite[] playerIcons = StaticData.Instance.PlayerIcons;
        for (int i = 0; i < playerIcons.Length; i++)
        {
            GameObject spriteItem = Instantiate(_iconItemPrefab, _iconItemsParent).gameObject;
            int cl = i;
            spriteItem.GetComponent<Button>().onClick.AddListener(delegate { SetPlayerSpriteID(cl); });
            spriteItem.GetComponent<Image>().sprite = playerIcons[i];
            _iconItems.Add(spriteItem);
        }
    }

    private void LeaveRoom() 
    {
        PhotonNetwork.LeaveRoom(true);
    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.SetPlayerCustomProperties(null);
        LobbyManager.Instance.SwitchToBrowseMenu();
        PhotonNetwork.LeaveLobby();
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            _startButton.onClick.AddListener(StartGame);
            _startButton.interactable = true;
            _startBalanceInput.interactable = true;
        }
        else
        {
            _startButton.interactable = false;
            _startBalanceInput.interactable = false;
        }

        PhotonNetwork.CurrentRoom.IsOpen = PhotonNetwork.PlayerList.Length < 4;

        CreatePlayerCustomProperties();
        UpdatePlayerList();
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        UpdatePlayerList();
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player player)
    {
        UpdatePlayerList();
    }

    private bool IsGameCanBeStarted() 
    {
        if (PhotonNetwork.PlayerList.Length < 2) 
        {
            return false;
        }

        int team1 = (int)PhotonNetwork.PlayerList[0].CustomProperties["Team"];
        for (int i = 1; i< PhotonNetwork.PlayerList.Length; i++) //checks if there is at least 2 different teams
        {
            if (!PhotonNetwork.PlayerList[i].CustomProperties.ContainsKey("Team")) 
            {
                return false;
            }
            if ((int)PhotonNetwork.PlayerList[i].CustomProperties["Team"] != team1) 
            {
                return true;
            }
        }
        return false;
    }

    public void StartGame() 
    {
        if (IsGameCanBeStarted())
        {
            float startBalance = GamePropertiesController.GameProperties.StartPlayerBalance;
            try
            {
                startBalance = int.Parse(_startBalanceInput.text);
            }
            catch (System.Exception)
            {
                startBalance = GamePropertiesController.GameProperties.StartPlayerBalance;
            }
            GamePropertiesController.GameProperties.StartPlayerBalance = startBalance;
            PhotonNetwork.CurrentRoom.IsVisible = false;
            PhotonNetwork.LoadLevel(3);
        }
    }

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

    public void SetPlayerTeam(int team)
    {
        _props["Team"] = team;
        PhotonNetwork.SetPlayerCustomProperties(_props);
    }

    public void SetPlayerIconInList(PlayerInLobbyItemPhoton playerItem, int spriteID) 
    {
        playerItem.SetIcon(StaticData.Instance.PlayerIcons[spriteID]);
    }

    public void SetPlayerColorInList(PlayerInLobbyItemPhoton playerItem, int colorID)
    {
        playerItem.SetColor(StaticData.Instance.AvatarColors[colorID]);
    }

    public void SetPlayerTeamInList(PlayerInLobbyItemPhoton playerItem, int team)
    {
        playerItem.SetTeam(team);
    }

    private void UpdatePlayerList()
    {
        if (PhotonNetwork.CurrentRoom == null)
        {
            return;
        }

        for (int i = 0; i < _playerInLobbyItems.Count; i++)
        {
            Destroy(_playerInLobbyItems[i].gameObject);
        }
        _playerInLobbyItems = new List<PlayerInLobbyItemPhoton>();

        foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
        {
            PlayerInLobbyItemPhoton playerItem = Instantiate(_playerItemPrefab, _playerItemsParent).GetComponent<PlayerInLobbyItemPhoton>();
            playerItem.Init(player.NickName, StaticData.Instance.AvatarColors[0], StaticData.Instance.PlayerIcons[0], this);
            _playerInLobbyItems.Add(playerItem);

            if (player.CustomProperties.ContainsKey("Color")) 
            {
                SetPlayerColorInList(playerItem, (int)player.CustomProperties["Color"]);
            }
            if (player.CustomProperties.ContainsKey("Icon"))
            {
                SetPlayerIconInList(playerItem, (int)player.CustomProperties["Icon"]);
            }
        }
    }

    private void CreatePlayerCustomProperties() 
    {
        _props["Color"] = 0;
        _props["Icon"] = 0;
        _props["Team"] = 0;
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
                if (changedProps.ContainsKey("Icon")) 
                {
                    SetPlayerIconInList(_playerInLobbyItems[i], (int)changedProps["Icon"]);
                }
                if (changedProps.ContainsKey("Team") && targetPlayer != PhotonNetwork.LocalPlayer)
                {
                    SetPlayerTeamInList(_playerInLobbyItems[i], (int)changedProps["Team"]);
                }
            }
        }
    }
}