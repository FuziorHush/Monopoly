public class DecksControllerLocal : DecksController
{
    public override void TriggerChanceDeck(Player player)
    {
        int triggeredId = _chanceDeck.TriggerRandomAction(player);
        _chanceDeck.TranslateActionTrigger(triggeredId, player);
    }

    public override void TriggerLuckDeck(Player player)
    {
        int triggeredId = _luckDeck.TriggerRandomAction(player);
        _luckDeck.TranslateActionTrigger(triggeredId, player);

    }
}
