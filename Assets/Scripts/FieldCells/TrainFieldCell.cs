using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainFieldCell : FieldCell
{
    public override void Init()
    {
    }

    public override void Clicked(Player player)
    {
        if (GameFlowController.Instance.PlayerWhoTurn.CellOn == CellID || !GameFlowController.Instance.CurrentPlayerCanUseTrain)
            return;

        FieldController.Instance.GoOnCellByID(player, CellID);
        GameFlowController.Instance.CurrentPlayerCanUseTrain = false;
    }

    public override void Interact(Player player)
    {
        GameFlowController.Instance.CurrentPlayerCanUseTrain = true;
    }

    protected override void ShowInfo()
    {
        InfoMenu.Instance.ShowInfoTrain();
    }
}
