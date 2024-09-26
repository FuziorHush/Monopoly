using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HandOverPlayerUIItem : MonoBehaviour
{
    [SerializeField] private TMP_Text _nickname;
    [SerializeField] private Button _give1;
    [SerializeField] private Button _give2;
    [SerializeField] private TMP_Text _give1Text;
    [SerializeField] private TMP_Text _give2Text;
    [SerializeField] private float _amount1;
    [SerializeField] private float _amount2;

    public void Init(HandOverMoneyMenu menu, float playerBalance, Player targetPlayer) {
        _nickname.text = targetPlayer.NetworkPlayer.NickName;
        _give1Text.text = _amount1 + "$";
        _give2Text.text = _amount2 + "$";
        if (playerBalance >= _amount1)
        {
            _give1.interactable = true;
            _give1.onClick.AddListener(delegate { menu.HandOverMoney(targetPlayer, _amount1); });
        }
        if (playerBalance >= _amount2)
        {
            _give2.interactable = true;
            _give2.onClick.AddListener(delegate { menu.HandOverMoney(targetPlayer, _amount2); });
        }
    }
}
