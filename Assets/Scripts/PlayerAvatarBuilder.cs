using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAvatarBuilder : MonoBehaviour
{
    [SerializeField] private Sprite[] _sprites;
    private AvatarColor[] _avatarColors;

    [SerializeField] private GameObject _baseAvatar;

    private void Awake()
    {
        _avatarColors = Resources.LoadAll<AvatarColor>("AvatarColors");
    }

    public GameObject CreateAvatar(int spriteID, int colorID) 
    {
        GameObject avatar = Instantiate(_baseAvatar);
        SpriteRenderer frontRenderer = avatar.transform.GetChild(0).GetComponent<SpriteRenderer>();
        frontRenderer.sprite = _sprites[spriteID];
        frontRenderer.color = _avatarColors[colorID].FrontColor;
        avatar.transform.GetChild(1).GetComponent<SpriteRenderer>().color = _avatarColors[colorID].BackColor;
        return avatar;
    }
}
