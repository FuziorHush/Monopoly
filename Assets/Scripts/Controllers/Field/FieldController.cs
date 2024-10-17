using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public abstract class FieldController : MonoSingleton<FieldController>
{
    protected GameFieldStaticData _fieldData;
    protected Vector3 _startCellPos;
    protected int _cellsNum;

    protected Player _actingPlayer;
    protected FieldCell _targetCell;

    protected override void Awake()
    {
        base.Awake();
        GameEvents.ControllersCreated += Init;
    }

    public void Init()
    {
        _fieldData = GameFieldStaticData.Instance;
        _startCellPos = _fieldData._cells[0].transform.position;
        _cellsNum = _fieldData._cells.Length;
        for (int i = 0; i < _fieldData._cells.Length; i++)
        {
            _fieldData._cells[i].Init();
            _fieldData._cells[i].CellID = i;
        }
    }

    public abstract Transform CreatePlayerAvatar(int number, Photon.Realtime.Player networkPlayer = null);
    public abstract void GoOnCellByID(Player player, int cell);
    public abstract void GoForward(Player player, int steps);
    public abstract void GoBackward(Player player, int steps);
}
