using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Team
{
    public static int MAX_PLAYERS_IN_TEAM = 3;

    public Team(string name) {
        Name = name;
    }

    public string Name { get; private set; }
    private List<Player> _players = new List<Player>();
    public int NumPlayers => _players.Count;

    public bool AddPlayer(Player player)
    {
        if (!_players.Contains(player) && _players.Count < MAX_PLAYERS_IN_TEAM) {
            _players.Add(player);
            player.Team = this;
            return true;
        }
        return false;
    }

    public bool RemovePlayer(Player player)
    {
        if (_players.Contains(player))
        {
            _players.Remove(player);
            player.Team = null;
            return true;
        }
        return false;
    }

    public Player this[int i] {
        get => _players[i];
    }

    public bool Contains(Player player)
    {
        return _players.Contains(player);
    }
}
