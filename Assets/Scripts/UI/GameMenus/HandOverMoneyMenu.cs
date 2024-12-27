using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class HandOverMoneyMenu : MonoSingleton<HandOverMoneyMenu>
{
    [SerializeField] private GameObject _windowGameObject;
    [SerializeField] private Transform _playerItemsContainer;
    [SerializeField] private GameObject _playerItemPrefab;
    [SerializeField] private TMP_Text _messageText;
    private List<HandOverPlayerUIItem> _items;

    [SerializeField] private float _windowOffset;
    [SerializeField] private float _windowAnimationTime;

    public bool MenuIsOpen { get; private set; }

    private Player _targetPlayer;

    protected override void Awake()
    {
        base.Awake();
        GameEvents.NewTurn += OnNewTurn;
        GameEvents.DicesTossed += OnDicesTossed;

        _windowGameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(_windowOffset, 0, 0);
    }

    public void Open(Player targetPlayer)
    {
        _windowGameObject.transform.DOLocalMoveX(0, _windowAnimationTime);
        _targetPlayer = targetPlayer;
        _items = new List<HandOverPlayerUIItem>();

        for (int i = 0; i < GameFlowController.Instance.Players.Count; i++)
        {
            Player player = GameFlowController.Instance.Players[i];
            if (player != _targetPlayer && player.Team == _targetPlayer.Team) 
            {
                HandOverPlayerUIItem playerUIItem = Instantiate(_playerItemPrefab, _playerItemsContainer).GetComponent<HandOverPlayerUIItem>();
                playerUIItem.Init(targetPlayer, player);
                _items.Add(playerUIItem);
            }
        }

        if (_items.Count == 0)
        {
            _messageText.text = LanguageSystem.Instance["handovermenu_noallies"];
        }
        else 
        {
            _messageText.text = LanguageSystem.Instance["handovermenu_transfer"];
        }

        MenuIsOpen = true;
    }

    public void CloseMenu() 
    {
        if (_items != null)
        {
            for (int i = 0; i < _items.Count; i++)
            {
                _items[i].DisableButtons();
            }
        }
        _windowGameObject.transform.DOLocalMoveX(_windowOffset, _windowAnimationTime).OnComplete(DestroyElements);
        MenuIsOpen = false;
    }

    private void DestroyElements() 
    {
        if (_items != null)
        {
            for (int i = 0; i < _items.Count; i++)
            {
                Destroy(_items[i].gameObject);
            }
            _items = null;
        }
    }

    private void OnNewTurn(Player player) {
        if(MenuIsOpen)
        CloseMenu();
    }

    private void OnDicesTossed(Vector2Int result)
    {
        CloseMenu();
    }
}
