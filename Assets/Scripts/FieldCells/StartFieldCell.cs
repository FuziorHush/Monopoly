using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartFieldCell : FieldCell
{
    public override void Interact(Player player)
    {
        GameEvents.PlayerFieldCellInteractionEnded(player, true);
    }

    protected override void ShowInfo()
    {
        InfoMenu.Instance.ShowInfoStart();
    }
}
