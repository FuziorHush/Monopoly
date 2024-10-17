using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EstateFieldCell : FieldCell
{
    [SerializeField] private string _name;
    private Estate _estate;
    private SpriteRenderer _buildingsSprite;

    public override void Init()
    {
        EstateData estateData = GamePropertiesController.Instance.GameProperties.Estates.Find(x => x.Name == _name);
         _estate = new Estate(estateData.Name, estateData.BaseQuantity, this);
        EstatesController.Instance.Estates.Add(_estate);
        _buildingsSprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
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
            if (_estate.OlympBonusApplied)
            {
                taxValue = Mathf.RoundToInt(taxValue * OlympController.Instance.CurrentOlympBonus);
            }

            BalancesController.Instance.Transferbalance(player, _estate.Owner, taxValue);
        }
    }

    protected override void ShowInfo()
    {
        InfoMenu.Instance.ShowInfoEstate(_estate);
    }

    public override void Clicked(Player player)
    {
        if (GameFlowController.Instance.PlayerWhoTurn.Balance < 0)
        {
            float pledge = Mathf.RoundToInt(_estate.CurrentQuantity / 2);
            _estate.PledgedAmount = pledge;
            GameFlowController.Instance.PlayerWhoTurn.Balance += pledge;
        }
        else if (OlympController.Instance.CanApplyOlympBonus && _estate.Owner == player)
        {
            OlympController.Instance.SetBonusToEstate(_estate);
        }
    }

    public void UpdateBuildingSprite() {
        _buildingsSprite.color = _estate.Owner.AvatarColor.FrontColor;
        if (_estate.Level == 0)
        {
            _buildingsSprite.sprite = null;
        }
        else {
            _buildingsSprite.sprite = StaticData.Instance.BuildingsSprites[_estate.Level - 1];
        }
    }
}
