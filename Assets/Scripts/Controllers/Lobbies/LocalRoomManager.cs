using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LocalRoomManager : MonoBehaviour
{
    [SerializeField] private Button _leaveButton;
    [SerializeField] private Transform _playerItemsParent;
    [SerializeField] private GameObject _playerItemPrefab;

    [SerializeField] private GameObject _colorItemPrefab;
    [SerializeField] private GameObject _iconItemPrefab;

    [SerializeField] private TMPro.TMP_InputField _startBalanceInput;
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _addPlayerButton;

    private List<PlayerInLobbyItemLocal> _playerInLobbyItems = new List<PlayerInLobbyItemLocal>();
    [SerializeField] private Transform _colorItemsParent;
    [SerializeField] private Transform _iconItemsParent;
    private List<GameObject> _colorItems = new List<GameObject>();
    private List<GameObject> _iconItems = new List<GameObject>();

    [SerializeField] private Color _selectedColor;

    private LocalGameData _localGameData;
    private PlayerInLobbyItemLocal _playerInLobbyItemToConfigure;

    private void Awake()
    {
        _leaveButton.onClick.AddListener(LeaveRoom);
        _startBalanceInput.text = GamePropertiesController.GameProperties.StartPlayerBalance.ToString();
        _startButton.onClick.AddListener(StartGame);
        _addPlayerButton.onClick.AddListener(AddLocalPlayer);

        AvatarColor[] avatarColors = StaticData.Instance.AvatarColors;
        for (int i = 0; i < avatarColors.Length; i++)
        {
            GameObject colorItem = Instantiate(_colorItemPrefab, _colorItemsParent).gameObject;
            int cl = i;
            colorItem.GetComponent<Button>().onClick.AddListener(delegate { SetPlayerColorInList(cl); });
            colorItem.transform.GetChild(0).GetComponent<Image>().color = avatarColors[i].FrontColor;
            _colorItems.Add(colorItem);
        }

        Sprite[] playerIcons = StaticData.Instance.PlayerIcons;
        for (int i = 0; i < playerIcons.Length; i++)
        {
            GameObject spriteItem = Instantiate(_iconItemPrefab, _iconItemsParent).gameObject;
            int cl = i;
            spriteItem.GetComponent<Button>().onClick.AddListener(delegate { SetPlayerIconInList(cl); });
            spriteItem.GetComponent<Image>().sprite = playerIcons[i];
            _iconItems.Add(spriteItem);
        }
    }

    private void LeaveRoom()
    {
        LobbyManager.Instance.SwitchToBrowseMenu();
        Destroy(_localGameData.gameObject);
    }

    public void OpenInit()
    {
        GameObject _localGameDataContainer = new GameObject("LocalGameDataContainer");
        _localGameData = _localGameDataContainer.AddComponent<LocalGameData>();
        _localGameData.AddPlayer();
        UpdatePlayerList();
        SetPlayerIUItemToConfigure(_playerInLobbyItems[0]);
        _startButton.interactable = false;
    }

    public void AddLocalPlayer() 
    {
        if (_localGameData.Players.Count == 4)
            return;

        _localGameData.AddPlayer();
        UpdatePlayerList();
        SetPlayerIUItemToConfigure(_playerInLobbyItems[0]);
        SetStartButtonInteractableIfGameCanBeStarted();
    }

    public void RemoveLocalPlayer(RoomPlayerData player)
    {
        _localGameData.RemovePlayer(player);
        UpdatePlayerList();
        SetPlayerIUItemToConfigure(_playerInLobbyItems[0]);
        SetStartButtonInteractableIfGameCanBeStarted();
    }

    public void SetPlayerIUItemToConfigure(PlayerInLobbyItemLocal item) 
    {
        _playerInLobbyItemToConfigure = item;
    }

    public void SetStartButtonInteractableIfGameCanBeStarted()
    {
        if (_localGameData.Players.Count < 2)
        {
            _startButton.interactable = false;
            return;
        }

        int team1 = _localGameData.Players[0].Team;
        for (int i = 1; i < _localGameData.Players.Count; i++) //checks if there is at least 2 different teams
        {
            if (_localGameData.Players[i].Team != team1)
            {
                _startButton.interactable = true;
                return;
            }
        }
        _startButton.interactable = false;
    }

    public void StartGame()
    {
        float startBalance = GamePropertiesController.GameProperties.StartPlayerBalance;
        try
        {
            startBalance = int.Parse(_startBalanceInput.text);
        }
        catch (System.Exception)
        {

        }
        GamePropertiesController.GameProperties.StartPlayerBalance = startBalance;

        SceneManager.LoadScene(2);
    }

    public void SetPlayerIconInList(int spriteID)
    {
        _playerInLobbyItemToConfigure._roomPlayer.Icon = StaticData.Instance.PlayerIcons[spriteID];
        _playerInLobbyItemToConfigure.SetIcon(StaticData.Instance.PlayerIcons[spriteID]);
    }

    public void SetPlayerColorInList(int colorID)
    {
        _playerInLobbyItemToConfigure._roomPlayer.AvatarColor = StaticData.Instance.AvatarColors[colorID];
        _playerInLobbyItemToConfigure.SetColor(StaticData.Instance.AvatarColors[colorID]);
    }

    private void UpdatePlayerList()
    {
        _playerInLobbyItems = new List<PlayerInLobbyItemLocal>();
        for (int i = 0; i < _playerItemsParent.childCount; i++)
        {
            Destroy(_playerItemsParent.GetChild(i).gameObject);
        }

        for (int i = 0; i < _localGameData.Players.Count; i++)
        {
            bool isMaster = i == 0;

            PlayerInLobbyItemLocal playerItem = Instantiate(_playerItemPrefab, _playerItemsParent).GetComponent<PlayerInLobbyItemLocal>();
            playerItem.Init(_localGameData.Players[i], this, isMaster);
            playerItem.SetTeam(_localGameData.Players[i].Team);
            _playerInLobbyItems.Add(playerItem);
        }
    }
}
