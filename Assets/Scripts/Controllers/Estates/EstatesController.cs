using System.Collections.Generic;

public abstract class EstatesController : MonoSingleton<EstatesController>
{
    public List<Estate> Estates = new List<Estate>();
    public List<float> Coefs { get; private set; }
    protected List<Player> _players;

    protected Player _targetPlayer;
    protected Estate _targetEstate;
    protected float[] _prices;

    protected override void Awake()
    {
        base.Awake();

        GameEvents.ControllersCreated += Init;

        Coefs = new List<float>();
        var coefsList = GamePropertiesController.GameProperties.TaxesCoefs;
        for (int i = 0; i < coefsList.Count; i++)
        {
            Coefs.Add(coefsList[i].Coef);
        }
    }

    private void Init() 
    {
        _players = GameFlowController.Instance.Players;
    }

    public void SetTarget(Player player, Estate estate) {
        _targetPlayer = player;
        _targetEstate = estate;
    }

    public void SetPrices(float[] prices) {
        _prices = prices;
    }

    public abstract void Purchase(int lvl);
    public abstract void EstateCellClicked(Player player, Estate estate);
    public abstract void PledgeTargetEstate();
    public abstract void UnpledgeTargetEstate();

    protected override void OnDestroy()
    {
        base.OnDestroy();

        GameEvents.ControllersCreated -= Init;
    }
}
