using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FieldCell : MonoBehaviour
{
    public int CellID { get; set; }

    public virtual void Init() {  }

    public abstract void Interact(Player player);
    protected abstract void ShowInfo();

    private void OnMouseUpAsButton()
    {
        Clicked(GameFlowController.Instance.PlayerWhoTurn);
    }

    public virtual void Clicked(Player player) 
    { 
    
    }

    private void OnMouseOver()
    {
        ShowInfo();
    }

    private void OnMouseExit()
    {
        InfoMenu.Instance.ClearInfo();
    }
}
