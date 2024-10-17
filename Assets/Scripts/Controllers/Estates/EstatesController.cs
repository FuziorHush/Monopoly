using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EstatesController : MonoSingleton<EstatesController>
{
    public List<Estate> Estates = new List<Estate>();
    protected Player _targetPlayer;
    protected Estate _targetEstate;
    protected float[] _prices;

    protected override void Awake()
    {
        base.Awake();
    }

    public void SetTarget(Player player, Estate estate, float[] prices) {
        _targetPlayer = player;
        _targetEstate = estate;
        _prices = prices;
    }

    public abstract void Purchase(int lvl);
}
