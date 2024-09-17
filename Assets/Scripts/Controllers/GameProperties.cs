using System.Collections.Generic;

[System.Serializable]
public class GameProperties
{
    public float StartPlayerBalance;
    public float LoopPayment;
    public List<EstateData> Estates = new List<EstateData>();
}

[System.Serializable]
public class EstateData {
    public string Name;
    public float BaseQuantity;
}
