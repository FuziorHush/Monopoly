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
}

[System.Serializable]
public class EstateData {
    public string Name;
    public float BaseQuantity;
}
