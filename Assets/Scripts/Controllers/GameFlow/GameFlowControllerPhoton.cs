using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Realtime;
using ExitGames.Client.Photon;
using Photon.Pun;

public class GameFlowControllerPhoton : GameFlowController, IOnEventCallback
{
    private Vector2Int _dicesResult;

    public override void CreatePlayers()
    {
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; i++)
        {
            PhotonNetwork.RaiseEvent((byte)PhotonEventCodes.CreateNextPlayer, null, raiseEventOptions, SendOptions.SendReliable);
        }
    }

    private void CreatePlayer_All() 
    {
        int playersCount = Players.Count;
        Photon.Realtime.Player networkPlayer = PhotonNetwork.PlayerList[playersCount];
        Transform playerAvatar = GameFieldStaticData.Instance.AvatarPositioning.CreatePlayerAvatar(playersCount, networkPlayer);
        Player player = new Player(PhotonNetwork.PlayerList[playersCount].NickName, playersCount, playerAvatar, StaticData.Instance.AvatarColors[(int)networkPlayer.CustomProperties["Color"]]);
        player.NetworkPlayer = networkPlayer;
        player.Balance = GamePropertiesController.GameProperties.StartPlayerBalance;
        player.CellOn = GameFieldStaticData.Instance._cells[0];
        GameFieldStaticData.Instance._cells[0].AddPlayerOnCell(player);

        if (networkPlayer == PhotonNetwork.LocalPlayer)
            ControllerOwner = player;

        Players.Add(player);
        AddPlayerToTeam(player, (int)networkPlayer.CustomProperties["Team"] + "");
        GameEvents.PlayerCreated?.Invoke(player);
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
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent((byte)PhotonEventCodes.MatchStarted, null, raiseEventOptions, SendOptions.SendReliable);
    }

    private void StartMatch_All()
    {
        InitFirstTurn();
        GameFieldStaticData.Instance.AvatarPositioning.PositionAvatrsAtStart();//cant be in init due to ping
        GameEvents.MatchStarted?.Invoke();
    }

    public override void MakeTurn()
    {
        CurrentPlayerCanUseTrain = false;
        DontInteractWithNextCell = false;

        byte diceResult1 = (byte)Random.Range(1, 7);
        byte diceResult2 = (byte)Random.Range(1, 7);
        _dicesResult = new Vector2Int(diceResult1, diceResult2);
        DicesActive = false;

        object[] content = new object[2] { diceResult1, diceResult2 };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent((byte)PhotonEventCodes.DicesTossed, content, raiseEventOptions, SendOptions.SendReliable);

        if (_dicesResult.x == _dicesResult.y)
        {
            _diceDoublesInTurn++;
            if (_diceDoublesInTurn == 3)
            {
                JailController.Instance.SendPlayerToJail(ControllerOwner);
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
        FieldController.Instance.GoForward(PlayerWhoTurn, _dicesResult.x + _dicesResult.y);
    }

    private void ShowDicesResult_All(Vector2Int dicesResult)
    {
        _dicesResult = dicesResult;
        GameEvents.DicesTossed?.Invoke(_dicesResult);
    }

    private void InitFirstTurn()
    {
        print("Started");
        PlayerWhoTurn = Players[0];
        _playerWhoTurnNum = 0;
    }

    public override void NextTurn()
    {
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent((byte)PhotonEventCodes.NextTurn, null, raiseEventOptions, SendOptions.SendReliable);
    }

    private void NextTurn_All() 
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
        object[] data = new object[1] { (byte)Players.IndexOf(player) };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent((byte)PhotonEventCodes.PlayerRemoved, data, raiseEventOptions, SendOptions.SendReliable);
    }

    private void RemovePlayer_All(Player player)
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
            StartCoroutine(EndGame());
        }
        else if (Teams.Count == 0)
        {
            GameEvents.MatchEnded?.Invoke(null);
            StartCoroutine(EndGame());
        }
        else
        {
            if(PhotonNetwork.IsMasterClient)
            NextTurn();
        }
    }

    public override void ExitMatch()
    {
        PhotonNetwork.LeaveRoom();
    }

    private IEnumerator EndGame()
    {
        yield return new WaitForSeconds(2);
        PhotonNetwork.LeaveRoom();
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
        if (photonEvent.Code == (byte)PhotonEventCodes.CreateNextPlayer)
        {
            CreatePlayer_All();
        }
        else if (photonEvent.Code == (byte)PhotonEventCodes.MatchStarted)
        {
            StartMatch_All();
        }
        else if (photonEvent.Code == (byte)PhotonEventCodes.DicesTossed)
        {
            object[] data = (object[])photonEvent.CustomData;
            byte diceResult1 = (byte)data[0];
            byte diceResult2 = (byte)data[1];
            ShowDicesResult_All(new Vector2Int(diceResult1, diceResult2));
        }
        else if (photonEvent.Code == (byte)PhotonEventCodes.NextTurn)
        {
            NextTurn_All();
        }
        else if (photonEvent.Code == (byte)PhotonEventCodes.PlayerRemoved)
        {
            object[] data = (object[])photonEvent.CustomData;
            Player player = Players[(byte)data[0]];
            RemovePlayer_All(player);
        }
    }
}
