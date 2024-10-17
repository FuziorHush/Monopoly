using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecksControllerLocal : DecksController
{
    public override void TriggerChanceDeck(Player player)
    {
        _chanceDeck.TriggerRandomAction(player);
    }

    public override void TriggerLuckDeck(Player player)
    {
        _luckDeck.TriggerRandomAction(player);
    }
}
