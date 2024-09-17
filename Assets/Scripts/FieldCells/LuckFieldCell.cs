using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuckFieldCell : FieldCell
{
    public override void Interact(Player player)
    {
        DecksController.Instance.TriggerLuckDeck(player);
    }

    protected override void ShowInfo()
    {
        InfoMenu.Instance.ShowInfoLuck();
    }
}
