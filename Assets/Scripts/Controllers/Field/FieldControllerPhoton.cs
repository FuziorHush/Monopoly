using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;

public class FieldControllerPhoton : FieldController, IOnEventCallback
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
        object[] data = new object[2];
        data[0] = (byte)_players.IndexOf(player);
        data[1] = (byte)steps;
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent((byte)PhotonEventCodes.PlayerGoForward, data, raiseEventOptions, SendOptions.SendReliable);
    }

    private void GoForward_All(Player player, int steps) 
    {
        _actingPlayer = player;
        int playerPreviousCell = player.CellOn.CellID;
        int targetCellId = player.CellOn.CellID + steps;
        if (targetCellId >= _cellsNum)
        {
            targetCellId -= _cellsNum;
            player.Loops++;

            if(player.NetworkPlayer == PhotonNetwork.LocalPlayer)//both methods GoForward_All and AddBalance call to all players
                BalancesController.Instance.AddBalance(player, _loopPayment);
        }
        _targetCell = _fieldData._cells[targetCellId];

        player.CellOn.RemovePlayerFromCell(player);
        player.CellOn = _targetCell;
        _targetCell.AddPlayerOnCell(player);

        GameEvents.PlayerMoved?.Invoke(player, playerPreviousCell, targetCellId);
        GameFieldStaticData.Instance.AvatarPositioning.MovePlayerAvatar(player, _fieldData._cells[targetCellId]);
    }

    public override void GoBackward(Player player, int steps)
    {
        object[] data = new object[2];
        data[0] = (byte)_players.IndexOf(player);
        data[1] = (byte)steps;
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent((byte)PhotonEventCodes.PlayerGoBackward, data, raiseEventOptions, SendOptions.SendReliable);
    }

    private void GoBackward_All(Player player, int steps)
    {
        _actingPlayer = player;
        int playerPreviousCell = player.CellOn.CellID;
        int targetCellId = player.CellOn.CellID - steps;
        _targetCell = _fieldData._cells[targetCellId];

        player.CellOn.RemovePlayerFromCell(player);
        player.CellOn = _targetCell;
        _targetCell.AddPlayerOnCell(player);

        GameEvents.PlayerMoved?.Invoke(player, playerPreviousCell, targetCellId);
        GameFieldStaticData.Instance.AvatarPositioning.MovePlayerAvatar(player, _fieldData._cells[targetCellId]);
    }

    protected override void OnAnimationEnd()
    {
        if (_actingPlayer == GameFlowController.Instance.ControllerOwner)
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

    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code == (byte)PhotonEventCodes.PlayerGoForward)
        {
            object[] data = (object[])photonEvent.CustomData;
            Player player = _players[(byte)data[0]];
            byte steps = (byte)data[1];
            GoForward_All(player, steps);
        }
        else if (photonEvent.Code == (byte)PhotonEventCodes.PlayerGoBackward)
        {
            object[] data = (object[])photonEvent.CustomData;
            Player player = _players[(byte)data[0]];
            byte steps = (byte)data[1];
            GoBackward_All(player, steps);
        }
    }
}
