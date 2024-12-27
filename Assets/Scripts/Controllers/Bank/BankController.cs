using System.Collections.Generic;

public abstract class BankController : MonoSingleton<BankController>
{
    protected List<Player> _players;

    protected override void Awake()
    {
        base.Awake();
        GameEvents.ControllersCreated += Init;
    }

    public abstract void HandOverMoney(Player playerSender, Player playerReciever, float amount);

    private void Init()
    {
        _players = GameFlowController.Instance.Players;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        GameEvents.ControllersCreated -= Init;
    }
}
