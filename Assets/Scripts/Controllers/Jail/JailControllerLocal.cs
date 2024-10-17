using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JailControllerLocal : JailController
{
    public override void SendPlayerToJail(Player player, int turns)
    {
        _jailedPlayers.Add(new JailedPlayer(player, turns));
        GameEvents.PlayerSentToJail?.Invoke(player);
    }

    public override bool CheckJail(Player player)
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
        else
        {
            return false;
        }
    }

    public override void GiveGOJ(Player player)
    {
        player.GOJHas++;
    }

    public override void UseGOJ(Player player)
    {
        JailedPlayer jailedPlayer = _jailedPlayers.Find(x => x.Player == player);
        if (jailedPlayer != null)
        {
            player.GOJHas--;
            _jailedPlayers.Remove(jailedPlayer);
            GameEvents.PlayerFreedFromJail?.Invoke(jailedPlayer.Player);
        }
    }
}
