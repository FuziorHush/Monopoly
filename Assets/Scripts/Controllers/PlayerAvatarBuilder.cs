using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAvatarBuilder : MonoBehaviour
{
    [SerializeField] private GameObject _baseAvatarPrefab;

    public GameObject CreateAvatar(int spriteID, int colorID) 
    {
        GameObject avatar = Instantiate(_baseAvatarPrefab);
        SpriteRenderer frontRenderer = avatar.transform.GetChild(0).GetComponent<SpriteRenderer>();
        frontRenderer.sprite = StaticData.Instance.PlayerIcons[spriteID];
        frontRenderer.color = StaticData.Instance.AvatarColors[colorID].FrontColor;
        avatar.transform.GetChild(1).GetComponent<SpriteRenderer>().color = StaticData.Instance.AvatarColors[colorID].BackColor;
        return avatar;
    }

    public GameObject CreateAvatar(Sprite sprite, AvatarColor color)
    {
        GameObject avatar = Instantiate(_baseAvatarPrefab);
        SpriteRenderer frontRenderer = avatar.transform.GetChild(0).GetComponent<SpriteRenderer>();
        frontRenderer.sprite = sprite;
        frontRenderer.color = color.FrontColor;
        avatar.transform.GetChild(1).GetComponent<SpriteRenderer>().color = color.BackColor;
        return avatar;
    }
}
