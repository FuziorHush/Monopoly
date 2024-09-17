using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChanceFieldCell : FieldCell
{
    public override void Interact(Player player)
    {
        DecksController.Instance.TriggerChanceDeck(player);
    }

    protected override void ShowInfo() 
    {
        InfoMenu.Instance.ShowInfoChance();
    }
}
