using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerInLobbyItem : MonoBehaviour
{
    public string Owner { get; protected set; }
    [SerializeField] protected TMP_Text _nameText;
    [SerializeField] protected TMP_Dropdown _teamDropdown;
    [SerializeField] protected Image _avatarPreviewForeground;
    [SerializeField] protected Image _avatarPreviewBackground;
    [SerializeField] protected Button _kickPlayerButton;

    public void SetColor(AvatarColor avatarColor)
    {
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
}
