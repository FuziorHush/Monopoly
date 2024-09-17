using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameDebug
{
    public class DebugTracker : MonoBehaviour
    {
        void Start()
        {
            GameEvents.PlayerMoved += OnPlayerMoved;
            GameEvents.PlayerBoughtEstate += OnPlayerBoughtEstate;
            GameEvents.PlayerUpgradedEstate += OnPlayerUpgradedEstate;
            GameEvents.PlayerPayedTaxesToPlayer += OnPlayerPayedTaxesToPlayer;
            GameEvents.PlayerSentToJail += OnPlayerSentToJail;
            GameEvents.PlayerFreedFromJail += OnPlayerFreedFromJail;
            GameEvents.CardTriggered += OnCardTriggered;
        }

        private void OnPlayerMoved(Player player, int prevPos, int newPos) {
            DevConsoleBehaviour.Instance.Log($"{player.Name} moved from {prevPos} to {newPos}");
        }

        private void OnPlayerBoughtEstate(Player player, Estate estate)
        {
            DevConsoleBehaviour.Instance.Log($"{player.Name} bought {estate.Name} (lvl{estate.Level})");
        }

        private void OnPlayerUpgradedEstate(Player player, Estate estate)
        {
            DevConsoleBehaviour.Instance.Log($"{player.Name} upgraded {estate.Name} (lvl{estate.Level})");
        }

        private void OnPlayerPayedTaxesToPlayer(Player player1, Player player2, float amount)
        {
            DevConsoleBehaviour.Instance.Log($"{player1.Name} paid {player2.Name} {amount}$");
        }

        private void OnPlayerSentToJail(Player player)
        {
            DevConsoleBehaviour.Instance.Log($"{player.Name} sent to jail");
        }

        private void OnPlayerFreedFromJail(Player player)
        {
            DevConsoleBehaviour.Instance.Log($"{player.Name} freed from jail");
        }

        private void OnCardTriggered(int cardID, CardDeck cardDeck, Player player) {
            DevConsoleBehaviour.Instance.Log($"{player.Name} triggered {cardID} from {cardDeck.ToString()}");
        }
    }
}
