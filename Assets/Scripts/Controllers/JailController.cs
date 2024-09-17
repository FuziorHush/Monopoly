using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JailController : MonoSingleton<JailController>
{
    private List<JailedPlayer> _jailedPlayers = new List<JailedPlayer>();

    protected override void Awake()
    {
        base.Awake();
    }

    public void SendPlayerToJail(Player player, int turns) {
        _jailedPlayers.Add(new JailedPlayer(player, turns));
        GameEvents.PlayerSentToJail?.Invoke(player);
    }

    public bool CheckJail(Player player) 
    {
        JailedPlayer jailedPlayer = _jailedPlayers.Find(x => x.Player == player);
        if (jailedPlayer != null)
        {
            jailedPlayer.TurnsLeft--;
            if (jailedPlayer.TurnsLeft == 0)
            {
                _jailedPlayers.Remove(jailedPlayer);
                GameEvents.PlayerFreedFromJail?.Invoke(jailedPlayer.Player);
                return false;
            }
            else
            {
                return true;
            }
        }
        else {
            return false;
        }
    }

    private class JailedPlayer {

        public Player Player;
        public int TurnsLeft;

        public JailedPlayer(Player player, int turns)
        {
            Player = player;
            TurnsLeft = turns;
        }
    }
}
