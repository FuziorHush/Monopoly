using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class JailController : MonoSingleton<JailController>
{
    protected List<Player> _players;

    protected List<JailedPlayer> _jailedPlayers = new List<JailedPlayer>();

    protected int _jailTurns;

    protected override void Awake()
    {
        base.Awake();
        GameEvents.ControllersCreated += Init;
    }

    private void Init()
    {
        _players = GameFlowController.Instance.Players;
        _jailTurns = GamePropertiesController.GameProperties.JailTurns;
    }

    public bool IsPlayerInJail(Player player) 
    {
        return _jailedPlayers.Find(x => x.Player == player) != null;
    }

    public string GetJailInfo()
    {
        string info = "";
        if (_jailedPlayers.Count == 0)
        {
            info += LanguageSystem.Instance["cellinfo_prison_empty"];
        }
        else
        {
            for (int i = 0; i < _jailedPlayers.Count; i++)
            {
                info += $"{_jailedPlayers[i].Player.Name} ({_jailedPlayers[i].TurnsLeft} {LanguageSystem.Instance["cellinfo_prison_turns"]})";
            }
        }
        return info;
    }

    public abstract void SendPlayerToJail(Player player);
    public abstract void TurnJail(Player player);
    public abstract void GiveGOJ(Player player);
    public abstract void UseGOJ(Player player);

    protected override void OnDestroy()
    {
        base.OnDestroy();

        GameEvents.ControllersCreated -= Init;
    }

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
