using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameFlowControllerLocal : GameFlowController
{
    public override void CreatePlayers()
    {
        float _startBalance = GamePropertiesController.GameProperties.StartPlayerBalance;
        for (int i = 0; i < LocalGameData.Instance.Players.Count; i++) 
        {
            int playersCount = Players.Count;
            Transform playerAvatar = GameFieldStaticData.Instance.AvatarPositioning.CreatePlayerAvatar(playersCount);
            Player player = new Player(LocalGameData.Instance.Players[i].Name, playersCount, playerAvatar, LocalGameData.Instance.Players[i].AvatarColor);
            player.Balance = _startBalance;
            player.CellOn = GameFieldStaticData.Instance._cells[0];
            GameFieldStaticData.Instance._cells[0].AddPlayerOnCell(player);

            Players.Add(player);
            AddPlayerToTeam(player, LocalGameData.Instance.Players[i].Team.ToString());
            GameEvents.PlayerCreated?.Invoke(player);
        }
        Destroy(LocalGameData.Instance.gameObject);
    }

    private void AddPlayerToTeam(Player player, string teamName)
    {
        Team team = Teams.Find(x => x.Name == teamName);
        if (team == null)
        {
            Team newTeam = new Team(teamName);
            Teams.Add(newTeam);
            newTeam.AddPlayer(player);
        }
        else
        {
            team.AddPlayer(player);
        }
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
            _diceDoublesInTurn++;
            if (_diceDoublesInTurn == 3)
            {
                JailController.Instance.SendPlayerToJail(PlayerWhoTurn);
                _diceDoublesInTurn = 0;
                NextTurn();
                return;
            }
            else
            {
                DicesActive = true;
                Instantiate(GameFieldStaticData.Instance._doubleDicesEffect, GameFieldStaticData.Instance._doubleDicesEffectSpawnPoint.position, Quaternion.identity);
            }
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

        if (JailController.Instance.IsPlayerInJail(PlayerWhoTurn))
        {
            DicesActive = false;
            JailController.Instance.TurnJail(PlayerWhoTurn);
            if (!JailController.Instance.IsPlayerInJail(PlayerWhoTurn))
            {
                DicesActive = true;
            }
        }
        else
        {
            DicesActive = true;
        }

        GameEvents.NewTurn?.Invoke(PlayerWhoTurn);
    }

    protected override void CheckIfPlayerCanPledge(Player player, float balance)
    {
        for (int i = 0; i < player.EstatesOwn.Count; i++)
        {
            if (player.EstatesOwn[i].PledgedAmount > 0)
                continue;

            balance += Mathf.RoundToInt((player.EstatesOwn[i].CurrentQuantity) / 2);
            if (balance >= 0)
            {
                return;
            }
        }

        RemovePlayer(player);
    }

    public override void RemovePlayer(Player player)
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

    public override void ExitMatch()
    {
        SceneManager.LoadScene(1);
    }

    private IEnumerator CloseRoom()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(1);
    }
}
