using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Estate
{
    public Estate(string name, float baseQuantity, EstateFieldCell cell) {
        Name = name;
        BaseQuantity = baseQuantity;
        TaxesCoef = 0.1f;
        CellLink = cell;
    }

    public string Name { get; private set; }
    public float BaseQuantity { get; private set; }

    public float CurrentQuantity { get; set; }
    public float TaxesCoef { get; set; }
    public Player Owner { get; set; }
    public int Level { get; set; }
    public float PledgedAmount { get; set; }
    public bool OlympBonusApplied { get; set; }

    public EstateFieldCell CellLink { get; private set; }

    public void ResetEstate() 
    {
        CurrentQuantity = 0;
        Owner = null;
        Level = 1;
        PledgedAmount = 0;

        GameEvents.EstateReseted?.Invoke(this);

            CellLink.UpdateBuildingSprite();
    }
}
