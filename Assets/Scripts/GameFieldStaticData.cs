using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFieldStaticData : MonoSingleton<GameFieldStaticData>
{
    protected override void Awake()
    {
        base.Awake();
    }

    public FieldCell[] _cells;
    public Vector2[] _playersPositionIndents;
    public GameObject _playerAvatarPrefab;
    public Transform _playerAvatarsParent;
    public PlayerAvatarBuilder _playerAvatarBuilder;
}
