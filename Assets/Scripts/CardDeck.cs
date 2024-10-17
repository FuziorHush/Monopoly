using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public abstract class CardDeck : MonoBehaviour
{
    protected delegate void CardAction();
    protected List<CardAction> _allCardActions = new List<CardAction>();
    protected List<CardAction> _currentCardActions;

    protected Player _targetPlayer;

    public int TriggerRandomAction(Player player) {
        _targetPlayer = player;
        CardAction cardAction = _currentCardActions[Random.Range(0, _currentCardActions.Count)];
        cardAction();
        return _allCardActions.IndexOf(cardAction);
    }

    public void TranslateActionTrigger(int cardActionIndex, Player player) {
        GameEvents.CardTriggered?.Invoke(cardActionIndex, this, player);

        _currentCardActions.Remove(_allCardActions[cardActionIndex]);

        if (_currentCardActions.Count == 0)
        {
            _currentCardActions = new List<CardAction>(_allCardActions);
        }
    }
}
