using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerInLobbyItem : MonoBehaviour
{
    public string Owner { get; private set; }
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Dropdown _teamDropdown;
    [SerializeField] private Image _avatarPreviewForeground;
    [SerializeField] private Image _avatarPreviewBackground;

    private RoomManager _roomManager;

    public void Init(string nickname, AvatarColor avatarColor, Sprite icon, RoomManager roomManager) 
    {
        _teamDropdown.onValueChanged.AddListener(delegate { OnTeamDropdownValueChanged(); });

        Owner = nickname;
        _nameText.text = nickname;
        _avatarPreviewForeground.color = avatarColor.FrontColor;
        _avatarPreviewBackground.color = avatarColor.BackColor;
        _avatarPreviewForeground.sprite = icon;
        _roomManager = roomManager;

        if (PhotonNetwork.NickName == nickname)
        {
            _teamDropdown.interactable = true;
        }
        else {
            _teamDropdown.interactable = false;
        }
    }

    public void SetColor(AvatarColor avatarColor) {
        _avatarPreviewForeground.color = avatarColor.FrontColor;
        _avatarPreviewBackground.color = avatarColor.BackColor;
    }

    public void SetIcon(Sprite icon)
    {
        _avatarPreviewForeground.sprite = icon;
    }

    public void SetTeam(int team)
    {
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
