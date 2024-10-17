using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FieldControllerLocal : FieldController
{
    public override Transform CreatePlayerAvatar(int number, Photon.Realtime.Player networkPlayer)
    {
        Vector3 spawnPos = new Vector3(_startCellPos.x + _fieldData._playersPositionIndents[number].x, _startCellPos.y + _fieldData._playersPositionIndents[number].y, 0);
        Transform playerAvatar = _fieldData._playerAvatarBuilder.CreateAvatar(number, number).transform;
        playerAvatar.position = spawnPos;
        playerAvatar.SetParent(_fieldData._playerAvatarsParent);
        return playerAvatar;
    }

    public override void GoOnCellByID(Player player, int cell)
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

    public override void GoForward(Player player, int steps)
    {
        _actingPlayer = player;
        int playerPreviousCell = player.CellOn;
        int targetCellId = player.CellOn + steps;
        if (targetCellId >= _cellsNum)
        {
            targetCellId -= _cellsNum;
            player.Loops++;
            BalancesController.Instance.AddBalance(player, GamePropertiesController.Instance.GameProperties.LoopPayment);
        }
        _targetCell = _fieldData._cells[targetCellId];
        player.CellOn = targetCellId;
        GameEvents.PlayerMoved?.Invoke(player, playerPreviousCell, targetCellId);
        MovePlayerAvatar(player, _fieldData._cells[targetCellId].transform);
    }

    public override void GoBackward(Player player, int steps)
    {
        _actingPlayer = player;
        int playerPreviousCell = player.CellOn;
        int targetCellId = player.CellOn - steps;
        _targetCell = _fieldData._cells[targetCellId];
        player.CellOn = targetCellId;
        GameEvents.PlayerMoved?.Invoke(player, playerPreviousCell, targetCellId);
        MovePlayerAvatar(player, _fieldData._cells[targetCellId].transform);
    }

    private void MovePlayerAvatar(Player player, Transform cellTransform)
    {
        GameEvents.MoveAnimationStarted?.Invoke();
        Vector3 targetPos = new Vector3(cellTransform.position.x + _fieldData._playersPositionIndents[player.Number].x, cellTransform.position.y + _fieldData._playersPositionIndents[player.Number].y, 0);
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
