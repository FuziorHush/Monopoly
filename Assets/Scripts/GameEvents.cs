using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class GameEvents
{
    public static UnityAction<Vector2Int> DicesTossed;
    public static UnityAction<Player> PlayerCreated;
    public static UnityAction MoveAnimationStarted;
    public static UnityAction MoveAnimationEnded;
    public static UnityAction<Player, int, int> PlayerMoved;
    public static UnityAction<Player, Estate> PlayerBoughtEstate;
    public static UnityAction<Player, Estate> PlayerUpgradedEstate;
    public static UnityAction<Player, Player, float> PlayerPayedTaxesToPlayer;
    public static UnityAction<Player, float, float> PlayerBalanceChanged;//player, value, currentBalance
    public static UnityAction<Player, float> PlayerBalanceIsNegative;
    public static UnityAction<Player, bool> PlayerFieldCellInteractionEnded;//player, next turn
    public static UnityAction<Player> NewTurn;
    public static UnityAction<Player> PlayerSentToJail;
    public static UnityAction<Player> PlayerFreedFromJail;
    public static UnityAction MatchStarted;
    public static UnityAction<int, CardDeck, Player> CardTriggered;
}