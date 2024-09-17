using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaxesFieldCell : FieldCell
{
    public override void Interact(Player player)
    {

    }

    protected override void ShowInfo()
    {
        InfoMenu.Instance.ShowInfoTaxes();
    }
}
