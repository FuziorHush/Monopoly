using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class OlympController : MonoSingleton<OlympController>
{
    //all
    public float CurrentOlympBonus { get; set; }
    protected Estate _estateWithBonusApplied;

    //client
    protected float _olympBonusMuiltiply = 1.1f;//TODO: take from properties
    public bool CanApplyOlympBonus { get; protected set; }

    protected override void Awake()
    {
        base.Awake();
        CurrentOlympBonus = _olympBonusMuiltiply;
        GameEvents.NewTurn += OnNextTurn;
    }

    public void ActivateBonus() 
    {
        CanApplyOlympBonus = true;
    }

    public abstract void SetBonusToEstate(Estate estate);

    private void OnNextTurn(Player player) {
        if (CanApplyOlympBonus)
        {
            CanApplyOlympBonus = false;
        }
    }
}
