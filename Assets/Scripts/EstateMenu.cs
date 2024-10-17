using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class EstateMenu : MonoSingleton<EstateMenu>
{
    [SerializeField] private Button[] _buyButtons;
    [SerializeField] private Button _dontBuyButton;
    [SerializeField] private TMP_Text[] _buyButtonsTexts;
    [SerializeField] private GameObject _windowGameObject;
    [SerializeField] private TMP_Text _estateName;

    public bool MenuIsOpen { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        for (int i = 1; i < 6; i++)
        {
            int cl = i;
            _buyButtons[i-1].onClick.AddListener(delegate {EstatesController.Instance.Purchase(cl); });
        }
        _dontBuyButton.onClick.AddListener(CloseMenu);
    }

    public void Init() 
    {
        _windowGameObject.SetActive(false);
    }

    public void Open(Player player, Estate estate)
    {
        _windowGameObject.SetActive(true);
        float[] prices = new float[5];
        float quantity = estate.BaseQuantity;

        _estateName.text = estate.Name;
        for (int i = 0; i < estate.Level; i++)
        {
            _buyButtons[i].interactable = false;
            _buyButtonsTexts[i].text = "приобретено";
            prices[i] = 0;
        }
        for (int i = estate.Level; i < 5; i++)
        {
            _buyButtons[i].interactable = player.Balance >= quantity;

            _buyButtonsTexts[i].text = quantity.ToString();
            prices[i] = quantity;
            quantity += estate.BaseQuantity;
        }

        EstatesController.Instance.SetTarget(player, estate, prices);
        MenuIsOpen = true;
    }

    public void CloseMenu() 
    {
        for (int i = 0; i < _buyButtons.Length; i++)
        {
            _buyButtons[i].interactable = false;
        }
        _windowGameObject.SetActive(false);

        MenuIsOpen = false;
    }
}
