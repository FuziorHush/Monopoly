using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalGameData : MonoSingleton<LocalGameData>
{
    public List<RoomPlayerData> Players = new List<RoomPlayerData>();
    private int _teamForNextPlayer;

    protected override void Awake()
    {
        DontDestroyOnLoad(gameObject);
        base.Awake();
    }

    public void AddPlayer()
    {
        RoomPlayerData data = new RoomPlayerData();
        data.Name = $"Player{Players.Count}";
        data.AvatarColor = StaticData.Instance.AvatarColors[Players.Count];
        data.Icon = StaticData.Instance.PlayerIcons[0];
        data.Team = _teamForNextPlayer;
        _teamForNextPlayer++;
        Players.Add(data);
    }

    public void RemovePlayer(RoomPlayerData player) 
    {
        Players.Remove(player);
        _teamForNextPlayer = 0;
    }
}

public class RoomPlayerData
{
    public string Name;
    public AvatarColor AvatarColor;
    public Sprite Icon;
    public int Team;
}


