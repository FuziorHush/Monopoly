using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class JailController : MonoSingleton<JailController>
{
    protected List<JailedPlayer> _jailedPlayers = new List<JailedPlayer>();

    protected override void Awake()
    {
        base.Awake();
    }

    public abstract void SendPlayerToJail(Player player, int turns);
    public abstract bool CheckJail(Player player);
    public abstract void GiveGOJ(Player player);
    public abstract void UseGOJ(Player player);

    public class JailedPlayer {

        public Player Player;
        public int TurnsLeft;

        public JailedPlayer(Player player, int turns)
        {
            Player = player;
            TurnsLeft = turns;
        }
    }
}
