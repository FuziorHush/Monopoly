using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Estate
{
    public Estate(string name, float baseQuantity) {
        Name = name;
        BaseQuantity = baseQuantity;
        TaxesCoef = 0.1f;
    }

    public string Name { get; private set; }
    public float BaseQuantity { get; private set; }

    public float CurrentQuantity { get; set; }
    public float TaxesCoef { get; set; }
    public Player Owner { get; set; }
    public int Level { get; set; }
}
