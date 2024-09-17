using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePropertiesController : MonoSingleton<GamePropertiesController>
{
    [SerializeField] private GamePropertiesLoader _gamePropertiesLoader;

    public GameProperties GameProperties { get; private set; }

    protected override void Awake()
    {
        base.Awake();
    }

    public void Init()
    {
        GameProperties = _gamePropertiesLoader.LoadProperties();
    }
}
