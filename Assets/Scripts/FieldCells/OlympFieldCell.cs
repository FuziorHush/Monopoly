using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OlympFieldCell : FieldCell
{
    public override void Init()
    {
    }

    public override void Interact(Player player)
    {

    }

    protected override void ShowInfo()
    {
        InfoMenu.Instance.ShowInfoOlymp();
    }
}
