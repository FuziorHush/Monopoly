using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class FieldController : MonoSingleton<FieldController>
{
    [SerializeField] private FieldCell[] _cells;
    [SerializeField] private Vector2[] _playersPositionIndents;
    [SerializeField] private GameObject _playerAvatarPrefab;
    [SerializeField] private Transform _playerAvatarsParent;
    [SerializeField] private PlayerAvatarBuilder _playerAvatarBuilder;
    private Vector3 _startCellPos;
    private int _cellsNum;

    private Player _actingPlayer;
    private FieldCell _targetCell;

    protected override void Awake()
    {
        base.Awake();
        _startCellPos = _cells[0].transform.position;
        _cellsNum = _cells.Length;
    }

    public void Init()
    {
        for (int i = 0; i < _cells.Length; i++)
        {
            _cells[i].Init();
            _cells[i].CellID = i;
        }
    }

    public Transform CreatePlayerAvatar(int number) 
    {
        Vector3 spawnPos = new Vector3(_startCellPos.x + _playersPositionIndents[number].x, _startCellPos.y + _playersPositionIndents[number].y, 0);
        Transform playerAvatar = _playerAvatarBuilder.CreateAvatar(number, number).transform;
        playerAvatar.position = spawnPos;
        playerAvatar.SetParent(_playerAvatarsParent);
        return playerAvatar;
    }

    public void GoOnCellByID(Player player, int cell)
    {
        int steps = -1;
        if (cell < player.CellOn)
        {
            steps = _cellsNum - player.CellOn + cell; //36 - 14 + 5
        }
        else if (cell == player.CellOn)
        {
            return;
        }
        else
        {
            steps = cell - player.CellOn;
        }
        GoForward(player, steps);
    }

    public void GoForward(Player player, int steps)
    {
        _actingPlayer = player;
        int playerPreviousCell = player.CellOn;
        int targetCellId = player.CellOn + steps;
        if (targetCellId >= _cellsNum) {
            targetCellId -= _cellsNum;
            player.Loops++;
            player.Balance += GamePropertiesController.Instance.GameProperties.LoopPayment;
        }
        _targetCell = _cells[targetCellId];
        player.CellOn = targetCellId;
        GameEvents.PlayerMoved?.Invoke(player, playerPreviousCell, targetCellId);
        MovePlayerAvatar(player, _cells[targetCellId].transform);
    }

    public void GoBackward(Player player, int steps)
    {
        _actingPlayer = player;
        int playerPreviousCell = player.CellOn;
        int targetCellId = player.CellOn - steps;
        _targetCell = _cells[targetCellId];
        player.CellOn = targetCellId;
        GameEvents.PlayerMoved?.Invoke(player, playerPreviousCell, targetCellId);
        MovePlayerAvatar(player, _cells[targetCellId].transform);
    }

    private void MovePlayerAvatar(Player player, Transform cellTransform)
    {
        GameEvents.MoveAnimationStarted?.Invoke();
        Vector3 targetPos = new Vector3(cellTransform.position.x + _playersPositionIndents[player.Number].x, cellTransform.position.y + _playersPositionIndents[player.Number].y, 0);
        player.AvatarTransform.DOMove(targetPos, 1).OnComplete(OnAnimationEnd);
    }

    private void OnAnimationEnd()
    {
        GameEvents.MoveAnimationEnded?.Invoke();

        if (GameFlowController.Instance.DontInteractWithNextCell)
        {
            GameFlowController.Instance.DontInteractWithNextCell = false;
        }
        else
        {
            _targetCell.Interact(_actingPlayer);
        }
    }
}
