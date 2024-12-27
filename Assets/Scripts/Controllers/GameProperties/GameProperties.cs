using System.Collections.Generic;

[System.Serializable]
public class GameProperties
{
    public string PropertiesVersion;

    public float StartPlayerBalance;
    public float LoopPayment;
    public float TaxesAmount;
    public float OlympMult;
    public int JailTurns;
    public List<EstateData> Estates = new List<EstateData>();
    public List<TaxesCoef> TaxesCoefs = new List<TaxesCoef>();
}

[System.Serializable]
public class EstateData 
{
    public string Name;
    public float BaseQuantity;
}

[System.Serializable]
public class TaxesCoef
{
    public int Level;
    public float Coef;
}
