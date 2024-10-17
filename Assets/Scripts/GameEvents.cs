using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class GameEvents
{
    public static UnityAction LanguageChanged;
    public static UnityAction ControllersCreated;

    public static UnityAction<Vector2Int> DicesTossed;
    public static UnityAction<Player> PlayerCreated;
    public static UnityAction MoveAnimationStarted;
    public static UnityAction MoveAnimationEnded;
    public static UnityAction<Player, int, int> PlayerMoved;
    public static UnityAction<Player, Estate> PlayerBoughtEstate;
    public static UnityAction<Player, Estate> PlayerUpgradedEstate;
    public static UnityAction<Player, Player, float> BalanceTransfered;
    public static UnityAction<Player, float> PlayerBalanceAdded;
    public static UnityAction<Player, float> PlayerBalanceWidthdrawn;
    public static UnityAction<Player, float> PlayerBalanceIsNegative;//current balance
    public static UnityAction<Player, bool> PlayerFieldCellInteractionEnded;//player, next turn
    public static UnityAction<Player> NewTurn;
    public static UnityAction<Player> PlayerSentToJail;
    public static UnityAction<Player> PlayerFreedFromJail;
    public static UnityAction MatchStarted;
    public static UnityAction<Team> MatchEnded;
    public static UnityAction<int, CardDeck, Player> CardTriggered;
    public static UnityAction<Estate> EstateUpdated;
    public static UnityAction<Estate> EstateReseted;
}
