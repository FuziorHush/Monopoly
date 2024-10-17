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
        Transform playerAvatar = FieldController.Instance.CreatePlayerAvatar(playersCount, networkPlayer);
        Player player = new Player(PhotonNetwork.PlayerList[playersCount].NickName, playersCount, playerAvatar, StaticData.Instance.AvatarColors[(int)networkPlayer.CustomProperties["Color"]]);
        player.NetworkPlayer = networkPlayer;

        if (networkPlayer == PhotonNetwork.LocalPlayer)
            ControllerOwner = player;

        Players.Add(player);
        GameEvents.PlayerCreated?.Invoke(player);
    }

    public override void StartMatch()
    {
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent((byte)PhotonEventCodes.MatchStarted, null, raiseEventOptions, SendOptions.SendReliable);
    }

    private void StartMatch_All()
    {
        InitFirstTurn();
        GameEvents.MatchStarted?.Invoke();
    }

    public override void MakeTurn()
    {
        CurrentPlayerCanUseTrain = false;
        DontInteractWithNextCell = false;

        Vector2Int dicesResult = new Vector2Int(Random.Range(1, 7), Random.Range(1, 7));
        DicesActive = false;

        object[] content = new object[1] { dicesResult };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent((byte)PhotonEventCodes.DicesTossed, content, raiseEventOptions, SendOptions.SendReliable);

        if (_dicesResult.x == _dicesResult.y)
        {
            if (_diceDoublesInTurn == 3)
            {
                //JailController.Instance.SendPlayerToJail(ControllerOwner, 3);
                _diceDoublesInTurn = 0;
                //NextTurn();
                return;
            }
            _diceDoublesInTurn++;
            DicesActive = true;
        }
        else
        {
            _diceDoublesInTurn = 0;
        }
        //FieldController.Instance.GoForward(PlayerWhoTurn, _dicesResult.x + _dicesResult.y);
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
            Vector2Int dicesResult = (Vector2Int)data[0];
            ShowDicesResult_All(dicesResult);
        }
    }
}
