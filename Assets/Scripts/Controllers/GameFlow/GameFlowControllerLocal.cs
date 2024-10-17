using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFlowControllerLocal : GameFlowController
{
    public override void CreatePlayers()
    {
       // for(int i = 0; i< )

        int playersCount = Players.Count;
        Transform playerAvatar = FieldController.Instance.CreatePlayerAvatar(playersCount);
        Player player = new Player("Player" + (playersCount + 1), playersCount, playerAvatar, StaticData.Instance.AvatarColors[playersCount]);

        Players.Add(player);
        GameEvents.PlayerCreated?.Invoke(player);
    }

    public override void StartMatch()
    {
        InitFirstTurn();
        GameEvents.MatchStarted?.Invoke();
    }

    public override void MakeTurn()
    {
        CurrentPlayerCanUseTrain = false;
        DontInteractWithNextCell = false;

        Vector2Int dicesValue = TossDices();
        DicesActive = false;
        if (dicesValue.x == dicesValue.y)
        {
            if (_diceDoublesInTurn == 3)
            {
                JailController.Instance.SendPlayerToJail(PlayerWhoTurn, 3);
                _diceDoublesInTurn = 0;
                NextTurn();
                return;
            }
            _diceDoublesInTurn++;
            DicesActive = true;
        }
        else
        {
            _diceDoublesInTurn = 0;
        }
        FieldController.Instance.GoForward(PlayerWhoTurn, dicesValue.x + dicesValue.y);
    }

    public Vector2Int TossDices()
    {
        Vector2Int dicesValue = new Vector2Int(Random.Range(1, 7), Random.Range(1, 7));
        GameEvents.DicesTossed?.Invoke(dicesValue);
        return dicesValue;
    }

    private void InitFirstTurn()
    {
        print("Started");
        PlayerWhoTurn = Players[0];
        _playerWhoTurnNum = 0;
    }

    public override void NextTurn()
    {
        CurrentPlayerCanUseTrain = false;

        _playerWhoTurnNum++;
        if (_playerWhoTurnNum == Players.Count)
        {
            _playerWhoTurnNum = 0;
        }
        PlayerWhoTurn = Players[_playerWhoTurnNum];

        DicesActive = !JailController.Instance.CheckJail(PlayerWhoTurn);

        GameEvents.NewTurn?.Invoke(PlayerWhoTurn);
    }

    protected override void CheckIfPlayerCanPledge(Player player, float balance)
    {
        for (int i = 0; i < player.EstatesOwn.Count; i++)
        {
            balance += Mathf.RoundToInt((player.EstatesOwn[i].CurrentQuantity) / 2);
            if (balance >= 0)
            {
                return;
            }
        }

        RemovePlayer(player);

        if (Teams.Count == 1)
        {
            GameEvents.MatchEnded?.Invoke(Teams[0]);
            StartCoroutine(CloseRoom());
        }
        else if (Teams.Count == 0)
        {
            GameEvents.MatchEnded?.Invoke(null);
            StartCoroutine(CloseRoom());
        }
        else
        {
            NextTurn();
        }
    }

    protected override void RemovePlayer(Player player)
    {
        for (int i = 0; i < player.EstatesOwn.Count; i++)
        {
            player.EstatesOwn[i].ResetEstate();
        }

        for (int i = 0; i < Teams.Count; i++)
        {
            if (Teams[i].Contains(player))
            {
                Teams[i].RemovePlayer(player);
                Teams.RemoveAt(i);
                break;
            }
        }

        Players.Remove(player);
        Destroy(player.AvatarTransform.gameObject);
    }

    private IEnumerator CloseRoom()
    {
        yield return new WaitForSeconds(2);
        //destroyRoom
    }
}
