using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JailControllerLocal : JailController
{
    public override void SendPlayerToJail(Player player)
    {
        _jailedPlayers.Add(new JailedPlayer(player, _jailTurns));
        GameEvents.PlayerSentToJail?.Invoke(player);
    }

    public override void TurnJail(Player player)
    {
        JailedPlayer jailedPlayer = _jailedPlayers.Find(x => x.Player == player);
        if (jailedPlayer != null)
        {
            jailedPlayer.TurnsLeft--;
            if (jailedPlayer.TurnsLeft == 0)
            {
                _jailedPlayers.Remove(jailedPlayer);
                GameEvents.PlayerFreedFromJail?.Invoke(jailedPlayer.Player);
            }
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
