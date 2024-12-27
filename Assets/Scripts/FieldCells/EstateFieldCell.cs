using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EstateFieldCell : FieldCell
{
    [SerializeField] private string _name;
    private Estate _estate;
    private SpriteRenderer _buildingsSprite;
    private SpriteRenderer _olympBoostedEffect;
    private SpriteRenderer _pledgedEffect;

    public override void Init()
    {
        EstateData estateData = GamePropertiesController.GameProperties.Estates.Find(x => x.Name == _name);
         _estate = new Estate(estateData.Name, estateData.BaseQuantity, this);
        EstatesController.Instance.Estates.Add(_estate);
        _buildingsSprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
        _olympBoostedEffect = transform.GetChild(1).GetChild(0).GetComponent<SpriteRenderer>();
        _pledgedEffect = transform.GetChild(1).GetChild(1).GetComponent<SpriteRenderer>();
    }

    public override void Interact(Player player)
    {
        if (_estate.PledgedAmount > 0)
            return;

        if (_estate.Owner == null || _estate.Owner == player || _estate.Owner.Team == player.Team)//free or player's or teammate's
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
        EstatesController.Instance.EstateCellClicked(player, _estate);
    }

    public void EnableOlympEffect()
    {
        _olympBoostedEffect.color = new Color(1f, 1f, 1f, 0.5f);
    }

    public void EnablePledgedEffect()
    {
        _pledgedEffect.color = Color.white;
    }

    public void DisableOlympEffect()
    {
        _olympBoostedEffect.color = Color.clear;
    }

    public void DisablePledgedEffect()
    {
        _pledgedEffect.color = Color.clear;
    }

    public void UpdateBuildingSprite() 
    {
        if (_estate.Owner != null)
        {
            _buildingsSprite.color = _estate.Owner.AvatarColor.FrontColor;
        }
        if (_estate.Level == 0)
        {
            _buildingsSprite.sprite = null;
        }
        else {
            _buildingsSprite.sprite = StaticData.Instance.BuildingsSprites[_estate.Level - 1];
        }
    }
}
