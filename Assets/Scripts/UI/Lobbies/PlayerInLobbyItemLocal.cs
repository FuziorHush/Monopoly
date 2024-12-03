using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInLobbyItemLocal : PlayerInLobbyItem
{
    public RoomPlayerData _roomPlayer;
    private LocalRoomManager _localRoomManager;

    [SerializeField] private Button _configureButton;

    public void Init(RoomPlayerData roomPlayer, LocalRoomManager localRoomManager, bool isMaster)
    {
        _kickPlayerButton.onClick.AddListener(RemovePlayer);
        _configureButton.onClick.AddListener(SetPlayerToConfigure);
        _teamDropdown.onValueChanged.AddListener(delegate { OnTeamDropdownValueChanged(); });

        _localRoomManager = localRoomManager;
        _roomPlayer = roomPlayer;
        Owner = roomPlayer.Name;
        _nameText.text = roomPlayer.Name;
        _avatarPreviewForeground.color = roomPlayer.AvatarColor.FrontColor;
        _avatarPreviewBackground.color = roomPlayer.AvatarColor.BackColor;
        _avatarPreviewForeground.sprite = roomPlayer.Icon;

        if (isMaster)
            _kickPlayerButton.gameObject.SetActive(false);
    }

    private void OnTeamDropdownValueChanged()
    {
        _roomPlayer.Team = _teamDropdown.value;
        _localRoomManager.SetStartButtonInteractableIfGameCanBeStarted();
    }

    private void RemovePlayer() 
    {
        _localRoomManager.RemoveLocalPlayer(_roomPlayer);
    }

    private void SetPlayerToConfigure()
    {
        _localRoomManager.SetPlayerIUItemToConfigure(this);
    }
}
