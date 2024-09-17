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
        _text.text = "";
    }

    public void ShowInfoStart()
    {
        _text.text = "Стартовая клетка";
    }

    public void ShowInfoEstate(Estate estate)
    {
        _sb.AppendLine(estate.Name);
        if (estate.Owner == null)
        {
            _sb.AppendLine("Нет владельца");
        }
        else
        {
            _sb.Append("Владелец: ");
            _sb.AppendLine(estate.Owner.Name);
        }

        if (estate.Owner != null)
        {
            if (GameFlowController.Instance.PlayerWhoTurn.Team != estate.Owner.Team)
            {
                _sb.Append("Попав на эту клетку вы заплатите ");
                _sb.Append("(не расчитано)");
                _sb.Append("$");
            }
        }
        _text.text = _sb.ToString();
        _sb.Clear();
    }

    public void ShowInfoChance()
    {
        _text.text = "Шанс";
    }

    public void ShowInfoLuck()
    {
        _text.text = "Удача";
    }

    public void ShowInfoTrain()
    {
        _text.text = "Поезд";
    }

    public void ShowInfoPrison()
    {
        _text.text = "Тюрьма";
    }

    public void ShowInfoBank()
    {
        _text.text = "Банк";
    }

    public void ShowInfoOlymp()
    {
        _text.text = "Олимпийские игры (медведев заснул)";
    }

    public void ShowInfoTaxes()
    {
        _text.text = "Налоги";
    }
}
