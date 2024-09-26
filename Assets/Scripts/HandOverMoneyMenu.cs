using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandOverMoneyMenu : MonoSingleton<HandOverMoneyMenu>
{
    [SerializeField] private GameObject _windowGameObject;
    [SerializeField] private Transform _playerItemsContainer;
    [SerializeField] private GameObject _playerItemPrefab;
    private List<HandOverPlayerUIItem> _items;

    public bool MenuIsOpen { get; private set; }

    private Player _targetPlayer;

    protected override void Awake()
    {
        base.Awake();
        GameEvents.NewTurn += OnNewTurn;
    }

    public void Open(Player targetPlayer)
    {
        _windowGameObject.SetActive(true);
        _targetPlayer = targetPlayer;
        _items = new List<HandOverPlayerUIItem>();

        for (int i = 0; i < GameFlowController.Instance.Players.Count; i++)
        {
            Player player = GameFlowController.Instance.Players[i];
            if (player != _targetPlayer && player.Team == _targetPlayer.Team) 
            {
                HandOverPlayerUIItem playerUIItem = Instantiate(_playerItemPrefab, _playerItemsContainer).GetComponent<HandOverPlayerUIItem>();
                playerUIItem.Init(this, targetPlayer.Balance, player);
            }
        }

        MenuIsOpen = true;
    }

    public void HandOverMoney(Player toPlayer, float amount) {
        if (_targetPlayer.Balance >= amount) 
        {
            _targetPlayer.Balance -= amount;
            toPlayer.Balance += amount;
        }
        CloseMenu();
    }

    public void CloseMenu() 
    {
        if (_items != null)
        {
            for (int i = 0; i < _items.Count; i++)
            {
                Destroy(_items[i].gameObject);
            }
            _items = null;
        }
        _windowGameObject.SetActive(false);
        MenuIsOpen = false;
    }

    private void OnNewTurn(Player player) {
        if(MenuIsOpen)
        CloseMenu();
    }
}
