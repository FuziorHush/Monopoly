using System.Collections.Generic;
using UnityEngine;

public abstract class FieldController : MonoSingleton<FieldController>
{
    protected List<Player> _players;

    protected GameFieldStaticData _fieldData;
    protected Vector3 _startCellPos;
    protected int _cellsNum;
    protected float _loopPayment;

    protected Player _actingPlayer;
    protected FieldCell _targetCell;

    protected override void Awake()
    {
        base.Awake();
        GameEvents.ControllersCreated += Init;
        GameEvents.MoveAnimationEnded += OnAnimationEnd;
    }

    public void Init()
    {
        _fieldData = GameFieldStaticData.Instance;
        _startCellPos = _fieldData._cells[0].transform.position;
        _cellsNum = _fieldData._cells.Length;
        _loopPayment = GamePropertiesController.GameProperties.LoopPayment;
        for (int i = 0; i < _fieldData._cells.Length; i++)
        {
            _fieldData._cells[i].Init();
            _fieldData._cells[i].CellID = i;
        }

        _players = GameFlowController.Instance.Players;
    }

    public abstract void GoOnCellByID(Player player, int cell);
    public abstract void GoForward(Player player, int steps);
    public abstract void GoBackward(Player player, int steps);
    protected abstract void OnAnimationEnd();

    protected override void OnDestroy()
    {
        base.OnDestroy();

        GameEvents.ControllersCreated -= Init;
        GameEvents.MoveAnimationEnded -= OnAnimationEnd;
    }
}
