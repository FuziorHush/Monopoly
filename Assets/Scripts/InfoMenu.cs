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
        _text.text = "��������� ������";
    }

    public void ShowInfoEstate(Estate estate)
    {
        _sb.AppendLine(estate.Name);
        if (estate.Owner == null)
        {
            _sb.AppendLine("��� ���������");
        }
        else
        {
            _sb.Append("��������: ");
            _sb.AppendLine(estate.Owner.Name);
        }

        if (estate.Owner != null)
        {
            if (GameFlowController.Instance.PlayerWhoTurn.Team != estate.Owner.Team)
            {
                _sb.Append("����� �� ��� ������ �� ��������� ");
                _sb.Append("(�� ���������)");
                _sb.Append("$");
            }
        }
        _text.text = _sb.ToString();
        _sb.Clear();
    }

    public void ShowInfoChance()
    {
        _text.text = "����";
    }

    public void ShowInfoLuck()
    {
        _text.text = "�����";
    }

    public void ShowInfoTrain()
    {
        _text.text = "�����";
    }

    public void ShowInfoPrison()
    {
        _text.text = "������";
    }

    public void ShowInfoBank()
    {
        _text.text = "����";
    }

    public void ShowInfoOlymp()
    {
        _text.text = "����������� ���� (�������� ������)";
    }

    public void ShowInfoTaxes()
    {
        _text.text = "������";
    }
}
