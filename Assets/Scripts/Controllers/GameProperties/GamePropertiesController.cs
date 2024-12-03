using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePropertiesController : MonoSingleton<GamePropertiesController>
{
    public static GameProperties GameProperties { get; private set; }

    protected override void Awake()
    {
        base.Awake();
    }

    public void Init(GameProperties properties)
    {
        GameProperties = properties;
        DontDestroyOnLoad(gameObject);
    }
}
