using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerInLobbyItem : MonoBehaviour
{
    public string Owner { get; private set; }
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Dropdown _teamDropdown;
    [SerializeField] private Image _avatarPreviewForeground;
    [SerializeField] private Image _avatarPreviewBackground;

    public void Init(string nickname, AvatarColor avatarColor, Sprite icon) 
    {
        Owner = nickname;
        _nameText.text = nickname;
        _avatarPreviewForeground.color = avatarColor.FrontColor;
        _avatarPreviewBackground.color = avatarColor.BackColor;
        _avatarPreviewForeground.sprite = icon;
    }

    public void SetColor(AvatarColor avatarColor) {
        _avatarPreviewForeground.color = avatarColor.FrontColor;
        _avatarPreviewBackground.color = avatarColor.BackColor;
    }

    public void SetIcon(Sprite icon)
    {
        _avatarPreviewForeground.sprite = icon;
    }
}
