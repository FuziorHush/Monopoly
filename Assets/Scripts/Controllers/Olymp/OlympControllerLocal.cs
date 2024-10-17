using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OlympControllerLocal : OlympController
{
    public override void SetBonusToEstate(Estate estate)
    {
        CanApplyOlympBonus = false;
        if (_estateWithBonusApplied != null)
        {
            _estateWithBonusApplied.OlympBonusApplied = false;
        }
        _estateWithBonusApplied = estate;
        estate.OlympBonusApplied = true;
        CurrentOlympBonus *= _olympBonusMuiltiply;
    }
}
