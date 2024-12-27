using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FieldCell : MonoBehaviour
{
    public int CellID { get; set; }
    public List<Player> PlayersOnCell { get; private set; } = new List<Player>();

    public virtual void Init() {  }

    public abstract void Interact(Player player);
    protected abstract void ShowInfo();

    private void OnMouseUpAsButton()
    {
        Clicked(GameFlowController.Instance.PlayerWhoTurn);
    }

    public void AddPlayerOnCell(Player player) {
        PlayersOnCell.Add(player);
    }

    public void RemovePlayerFromCell(Player player) {
        PlayersOnCell.Remove(player);
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
