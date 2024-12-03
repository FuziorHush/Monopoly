using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerInLobbyItemPhoton : PlayerInLobbyItem
{
    private RoomManagerPhoton _roomManager;

    public void Init(string nickname, AvatarColor avatarColor, Sprite icon, RoomManagerPhoton roomManager)
    {
        _teamDropdown.onValueChanged.AddListener(delegate { OnTeamDropdownValueChanged(); });

        Owner = nickname;
        _nameText.text = nickname;
        _avatarPreviewForeground.color = avatarColor.FrontColor;
        _avatarPreviewBackground.color = avatarColor.BackColor;
        _avatarPreviewForeground.sprite = icon;
        _roomManager = roomManager;

        if (PhotonNetwork.NickName == nickname || PhotonNetwork.IsMasterClient)
        {
            _teamDropdown.interactable = true;
        }
        else
        {
            _teamDropdown.interactable = false;
        }
    }

    public void SetValueToTeamDropdown(int team) {
        _teamDropdown.SetValueWithoutNotify(team);
    }

    public void OnTeamDropdownValueChanged() 
    {
        if (PhotonNetwork.NickName == Owner)
        {
            _roomManager.SetPlayerTeam(_teamDropdown.value);
        }
    }
}
