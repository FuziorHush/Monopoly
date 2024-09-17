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

    private Player _targetPlayer;
    private Estate _targetEstate;
    private float[] _prices;

    protected override void Awake()
    {
        base.Awake();

        for (int i = 1; i < 6; i++)
        {
            int cl = i;
            _buyButtons[i-1].onClick.AddListener(delegate { Purchase(cl); });
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

        _targetPlayer = player;
        _targetEstate = estate;
        _prices = new float[5];
        float quantity = _targetEstate.BaseQuantity;

        _estateName.text = estate.Name;
        for (int i = 0; i < estate.Level; i++)
        {
            _buyButtons[i].interactable = false;
            _buyButtonsTexts[i].text = "приобретено";
            _prices[i] = 0;
        }
        for (int i = estate.Level; i < 5; i++)
        {
            _buyButtons[i].interactable = _targetPlayer.Balance >= quantity;

            _buyButtonsTexts[i].text = quantity.ToString();
            _prices[i] = quantity;
            quantity += _targetEstate.BaseQuantity;
        }

        MenuIsOpen = true;
    }

    private void Purchase(int lvl) {
        if (_targetEstate.Owner == _targetPlayer)
        {
            UpgradeEstate(lvl);
        }
        else {
            BuyEstate(lvl);
        }

        CloseMenu();
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

    private void BuyEstate(int lvl) 
    {
        _targetPlayer.Balance -= _prices[lvl-1];
        _targetPlayer.EstatesOwn.Add(_targetEstate);
        _targetEstate.Owner = _targetPlayer;
        _targetEstate.CurrentQuantity = _prices[lvl-1];
        _targetEstate.Level = lvl;

        GameEvents.PlayerBoughtEstate?.Invoke(_targetPlayer, _targetEstate);
    }

    private void UpgradeEstate(int lvl) 
    {
        _targetPlayer.Balance -= _prices[lvl - 1];
        _targetEstate.CurrentQuantity += _prices[lvl - 1];
        _targetEstate.Level = lvl;

        GameEvents.PlayerUpgradedEstate?.Invoke(_targetPlayer, _targetEstate);
    }
}
