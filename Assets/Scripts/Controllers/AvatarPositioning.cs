using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AvatarPositioning : MonoBehaviour
{
    [SerializeField] private GameFieldStaticData _fieldData;

    [SerializeField] private Vector2[] _playersPositionIndents_1;
    [SerializeField] private Vector2[] _playersPositionIndents_2;
    [SerializeField] private Vector2[] _playersPositionIndents_3;
    [SerializeField] private Vector2[] _playersPositionIndents_4;
    private List<Vector2[]> _playersPositionIndents;

    [SerializeField] private float[] _playersScales;

    private void Awake()
    {
        _playersPositionIndents = new List<Vector2[]>();
        _playersPositionIndents.Add(_playersPositionIndents_1);
        _playersPositionIndents.Add(_playersPositionIndents_2);
        _playersPositionIndents.Add(_playersPositionIndents_3);
        _playersPositionIndents.Add(_playersPositionIndents_4);
    }

    public Transform CreatePlayerAvatar(int number)
    {
        Transform playerAvatar = _fieldData._playerAvatarBuilder.CreateAvatar(LocalGameData.Instance.Players[number].Icon, LocalGameData.Instance.Players[number].AvatarColor).transform;
        playerAvatar.SetParent(_fieldData._playerAvatarsParent);
        return playerAvatar;
    }

    public Transform CreatePlayerAvatar(int number, Photon.Realtime.Player networkPlayer)
    {
        Transform playerAvatar = _fieldData._playerAvatarBuilder.CreateAvatar((int)networkPlayer.CustomProperties["Icon"], (int)networkPlayer.CustomProperties["Color"]).transform;
        playerAvatar.SetParent(_fieldData._playerAvatarsParent);
        return playerAvatar;
    }

    public void PositionAvatrsAtStart() 
    {
        Vector3 startCellPos = _fieldData._cells[0].transform.position;
        List<Player> players = GameFlowController.Instance.Players;
        for (int i = 0; i < players.Count; i++)
        {
            players[i].AvatarTransform.position = new Vector3(startCellPos.x + _playersPositionIndents[players.Count - 1][i].x, startCellPos.y + _playersPositionIndents[players.Count - 1][i].y, 0);
            players[i].AvatarTransform.localScale = new Vector3(_playersScales[players.Count - 1], _playersScales[players.Count - 1], 1);
        }
    }

    public void MovePlayerAvatar(Player player, FieldCell previousCell) 
    {
        GameEvents.MoveAnimationStarted?.Invoke();

        int playersOnPrevCell = previousCell.PlayersOnCell.Count;
        Vector3 prevCellPos = previousCell.transform.position;
        for (int i = 0; i < playersOnPrevCell; i++)
        {
            Vector3 targetPos = new Vector3(prevCellPos.x + _playersPositionIndents[playersOnPrevCell - 1][i].x, prevCellPos.y + _playersPositionIndents[playersOnPrevCell - 1][i].y, 0);
            previousCell.PlayersOnCell[i].AvatarTransform.DOMove(targetPos, 0.5f);
            previousCell.PlayersOnCell[i].AvatarTransform.DOScale(new Vector3(_playersScales[playersOnPrevCell - 1], _playersScales[playersOnPrevCell - 1], 0), 0.5f);
        }


        int playersOnNewCell = player.CellOn.PlayersOnCell.Count;
        Vector3 newCellPos = player.CellOn.transform.position;
        for (int i = 0; i < playersOnNewCell; i++)
        {
            Vector3 targetPos = new Vector3(newCellPos.x + _playersPositionIndents[playersOnNewCell - 1][i].x, newCellPos.y + _playersPositionIndents[playersOnNewCell - 1][i].y, 0);

            player.CellOn.PlayersOnCell[i].AvatarTransform.DOScale(new Vector3(_playersScales[playersOnNewCell - 1], _playersScales[playersOnNewCell - 1], 0), 0.5f);
            if (player.CellOn.PlayersOnCell[i] != player)
            {
                player.CellOn.PlayersOnCell[i].AvatarTransform.DOMove(targetPos, 0.5f);
            }
            else
            {
                player.AvatarTransform.DOMove(targetPos, 1).OnComplete(delegate { GameEvents.MoveAnimationEnded?.Invoke(); });
            }
        }

    }
}
