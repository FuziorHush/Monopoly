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
        if (GameFlowController.Instance.PlayerWhoTurn.CellOn.CellID != CellID && GameFlowController.Instance.CurrentPlayerCanUseTrain)
        {
            FieldController.Instance.GoOnCellByID(player, CellID);
            GameFlowController.Instance.DontInteractWithNextCell = true;
            GameFlowController.Instance.CurrentPlayerCanUseTrain = false;
        }
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
