using System.Collections.Generic;

public abstract class DecksController : MonoSingleton<DecksController>
{
    protected List<Player> _players;

    protected ChanceDeck _chanceDeck;
    protected LuckDeck _luckDeck;

    protected override void Awake()
    {
        base.Awake();

        _chanceDeck = gameObject.AddComponent<ChanceDeck>();
        _luckDeck = gameObject.AddComponent<LuckDeck>();
        GameEvents.ControllersCreated += Init;
    }

    private void Init()
    {
        _players = GameFlowController.Instance.Players;
    }

    public abstract void TriggerChanceDeck(Player player);
    public abstract void TriggerLuckDeck(Player player);

    protected override void OnDestroy()
    {
        base.OnDestroy();

        GameEvents.ControllersCreated -= Init;
    }
}
