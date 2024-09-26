using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OlympController : MonoSingleton<OlympController>
{
    public float CurrentOlympBonus { get; set; }
    private float _olympBonusMuiltiply = 1.1f;
    private Estate _estateWithBonusApplied;
    public bool CanApplyOlympBonus { get; set; }

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

    public void SetBonusToEstate(Estate estate) {
        CanApplyOlympBonus = false;
        if (_estateWithBonusApplied != null) 
        {
            _estateWithBonusApplied.OlympBonusApplied = false;
        }
        _estateWithBonusApplied = estate;
        estate.OlympBonusApplied = true;
        CurrentOlympBonus *= _olympBonusMuiltiply;
    }

    private void OnNextTurn(Player player) {
        if (CanApplyOlympBonus)
        {
            CanApplyOlympBonus = false;
        }
    }
}
