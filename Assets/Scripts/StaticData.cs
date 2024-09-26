using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticData : MonoSingleton<StaticData>
{
    public Sprite[] PlayerIcons;
    [HideInInspector]public AvatarColor[] AvatarColors;
    public Sprite[] BuildingsSprites;

    protected override void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else {
            base.Awake();
            AvatarColors = Resources.LoadAll<AvatarColor>("AvatarColors");
            DontDestroyOnLoad(gameObject);
        }
    }
}
