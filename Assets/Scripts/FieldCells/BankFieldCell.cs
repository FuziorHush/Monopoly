using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BankFieldCell : FieldCell
{
    public override void Init()
    {

    }

    public override void Interact(Player player)
    {
        HandOverMoneyMenu.Instance.Open(player);
    }

    protected override void ShowInfo()
    {
        InfoMenu.Instance.ShowInfoBank();
    }

    public override void Clicked(Player player)
    {

    }
}
