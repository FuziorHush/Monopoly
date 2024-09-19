using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EstateFieldCell : FieldCell
{
    [SerializeField] private string _name;
    private Estate _estate;

    public override void Init()
    {
        EstateData estateData = GamePropertiesController.Instance.GameProperties.Estates.Find(x => x.Name == _name);
         _estate = new Estate(estateData.Name, estateData.BaseQuantity);
    }

    public override void Interact(Player player)
    {
        if (_estate.PledgedAmount > 0)
            return;

        if (_estate.Owner == null)
        {
            EstateMenu.Instance.Open(player, _estate);
        }
        else if (player.Team != _estate.Owner.Team)
        {
            float taxValue = Mathf.RoundToInt(_estate.CurrentQuantity * _estate.TaxesCoef);
            if (player.Balance >= taxValue)
            {
                player.Balance -= taxValue;
                _estate.Owner.Balance += taxValue;
            }
            else
            {
                _estate.Owner.Balance += player.Balance;
                player.Balance = 0;
            }

            GameEvents.PlayerPayedTaxesToPlayer?.Invoke(player, _estate.Owner, taxValue);
        }
    }

    protected override void ShowInfo()
    {
        InfoMenu.Instance.ShowInfoEstate(_estate);
    }

    public override void Clicked(Player player)
    {
        if (GameFlowController.Instance.PlayerWhoTurn.Balance < 0) {
            float pledge = Mathf.RoundToInt(_estate.CurrentQuantity / 2);
            _estate.PledgedAmount = pledge;
            GameFlowController.Instance.PlayerWhoTurn.Balance += pledge;
        }
    }
}
