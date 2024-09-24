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

    public void TriggerRandomAction(Player player) {
        _targetPlayer = player;
        CardAction cardAction = _currentCardActions[Random.Range(0, _currentCardActions.Count)];
        cardAction();
        print(_allCardActions.IndexOf(cardAction));
        GameEvents.CardTriggered?.Invoke(_allCardActions.IndexOf(cardAction), this, player);

        _currentCardActions.Remove(cardAction);

        if (_currentCardActions.Count == 0) {
            _currentCardActions = new List<CardAction>(_allCardActions);
        }
    }
}
