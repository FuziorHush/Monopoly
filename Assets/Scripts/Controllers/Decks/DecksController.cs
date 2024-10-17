using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DecksController : MonoSingleton<DecksController>
{
    protected ChanceDeck _chanceDeck;
    protected LuckDeck _luckDeck;

    protected override void Awake()
    {
        base.Awake();

        _chanceDeck = gameObject.AddComponent<ChanceDeck>();
        _luckDeck = gameObject.AddComponent<LuckDeck>();
    }

    public abstract void TriggerChanceDeck(Player player);
    public abstract void TriggerLuckDeck(Player player);
}
