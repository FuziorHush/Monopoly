using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecksController : MonoSingleton<DecksController>
{
    private ChanceDeck _chanceDeck;
    private LuckDeck _luckDeck;

    protected override void Awake()
    {
        base.Awake();

        _chanceDeck = GetComponent<ChanceDeck>();
        _luckDeck = GetComponent<LuckDeck>();
    }

    public void TriggerChanceDeck(Player player)
    {
        _chanceDeck.TriggerRandomAction(player);
    }

    public void TriggerLuckDeck(Player player)
    {
        _luckDeck.TriggerRandomAction(player);
    }
}
