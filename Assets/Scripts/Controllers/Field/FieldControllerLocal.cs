using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldControllerLocal : FieldController
{
    public override void GoOnCellByID(Player player, int cell)
    {
        int steps = -1;
        if (cell < player.CellOn.CellID)
        {
            steps = _cellsNum - player.CellOn.CellID + cell; //36 - 14 + 5
        }
        else if (cell == player.CellOn.CellID)
        {
            return;
        }
        else
        {
            steps = cell - player.CellOn.CellID;
        }
        GoForward(player, steps);
    }

    public override void GoForward(Player player, int steps)
    {
        _actingPlayer = player;
        int previousCellID = player.CellOn.CellID;
        FieldCell previousCell = player.CellOn;
        int targetCellId = player.CellOn.CellID + steps;
        if (targetCellId >= _cellsNum)
        {
            targetCellId -= _cellsNum;
            player.Loops++;
            BalancesController.Instance.AddBalance(player, _loopPayment);
        }
        _targetCell = _fieldData._cells[targetCellId];

        player.CellOn.RemovePlayerFromCell(player);
        player.CellOn = _targetCell;
        _targetCell.AddPlayerOnCell(player);

        GameEvents.PlayerMoved?.Invoke(player, previousCellID, targetCellId);
        GameFieldStaticData.Instance.AvatarPositioning.MovePlayerAvatar(player, previousCell);
    }

    public override void GoBackward(Player player, int steps)
    {
        _actingPlayer = player;
        int previousCellID = player.CellOn.CellID;
        FieldCell previousCell = player.CellOn;
        int targetCellId = player.CellOn.CellID - steps;
        _targetCell = _fieldData._cells[targetCellId];

        player.CellOn.RemovePlayerFromCell(player);
        player.CellOn = _targetCell;
        _targetCell.AddPlayerOnCell(player);

        GameEvents.PlayerMoved?.Invoke(player, previousCellID, targetCellId);
        GameFieldStaticData.Instance.AvatarPositioning.MovePlayerAvatar(player, previousCell);
    }

    protected override void OnAnimationEnd()
    {
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
