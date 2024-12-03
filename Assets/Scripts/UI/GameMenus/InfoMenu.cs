using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class InfoMenu : MonoSingleton<InfoMenu>
{
    [SerializeField] private TMPro.TMP_Text _text;
    private StringBuilder _sb = new StringBuilder();

    public void ClearInfo()
    {
        _text.text = LanguageSystem.Instance["cellinfo_empty"];
    }

    public void ShowInfoStart()
    {
        _sb.Append(LanguageSystem.Instance["cellinfo_start"]);
        _sb.Append(GamePropertiesController.GameProperties.StartPlayerBalance);
        _sb.Append("$");
        _text.text = _sb.ToString();
        _sb.Clear();
    }

    public void ShowInfoEstate(Estate estate)
    {
        _sb.AppendLine(estate.Name);
        if (estate.Owner == null)
        {
            _sb.AppendLine(LanguageSystem.Instance["cellinfo_estate_noowner"]);
        }
        else
        {
            _sb.Append(LanguageSystem.Instance["cellinfo_estate_owner"]);
            _sb.AppendLine(estate.Owner.Name);
        }

        if (estate.Owner != null)
        {
            if (GameFlowController.Instance.PlayerWhoTurn.Team != estate.Owner.Team && estate.PledgedAmount == 0)
            {
                _sb.Append(LanguageSystem.Instance["cellinfo_estate_youwillpay"]);
                if (estate.OlympBonusApplied)
                {
                    _sb.Append(Mathf.RoundToInt(estate.CurrentQuantity * estate.TaxesCoef * OlympController.Instance.CurrentOlympBonus));
                }
                else 
                {
                    _sb.Append(Mathf.RoundToInt(estate.CurrentQuantity * estate.TaxesCoef));
                }
                _sb.Append("$");
            }
        }

        if (estate.PledgedAmount > 0) {
            _sb.Append(LanguageSystem.Instance["cellinfo_estate_pledged"]);
            _sb.Append(estate.PledgedAmount);
            _sb.Append("$");
        }

        _text.text = _sb.ToString();
        _sb.Clear();
    }

    public void ShowInfoChance()
    {
        _text.text = LanguageSystem.Instance["cellinfo_chance"];
    }

    public void ShowInfoLuck()
    {
        _text.text = LanguageSystem.Instance["cellinfo_luck"];
    }

    public void ShowInfoTrain()
    {
        _text.text = LanguageSystem.Instance["cellinfo_train"];
    }

    public void ShowInfoPrison()
    {
        _text.text = LanguageSystem.Instance["cellinfo_prison"];
        _text.text += JailController.Instance.GetJailInfo();
    }

    public void ShowInfoBank()
    {
        _text.text = LanguageSystem.Instance["cellinfo_bank"];
    }

    public void ShowInfoOlymp()
    {
        _text.text = LanguageSystem.Instance["cellinfo_olymp"];
    }

    public void ShowInfoTaxes()
    {
        _sb.Append(LanguageSystem.Instance["cellinfo_taxes"]);
        _sb.Append("200");
        _sb.Append("$");
        _text.text = _sb.ToString();
        _sb.Clear();
    }
}
