using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class OlympMenu : MonoSingleton<OlympMenu>
{
    [SerializeField] private GameObject _windowGameObject;
    [SerializeField] private TMP_Text _messageText;

    [SerializeField] private float _windowOffset;
    [SerializeField] private float _windowAnimationTime;

    public bool MenuIsOpen { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        GameEvents.NewTurn += OnNewTurn;
        GameEvents.DicesTossed += OnDicesTossed;

        _windowGameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(_windowOffset, 0, 0);
    }

    public void Open()
    {
        _windowGameObject.transform.DOLocalMoveX(0, _windowAnimationTime);

        _messageText.text = $"выберите свою собственость (и запишите эту надпись в json)\nТекущий бонус: х{OlympController.Instance.CurrentOlympBonus}";

        MenuIsOpen = true;
    }

    public void CloseMenu()
    {
        _windowGameObject.transform.DOLocalMoveX(_windowOffset, _windowAnimationTime);
        MenuIsOpen = false;
    }

    private void OnNewTurn(Player player)
    {
        if (MenuIsOpen)
            CloseMenu();
    }

    private void OnDicesTossed(Vector2Int result)
    {
        if (MenuIsOpen)
            CloseMenu();
    }
}
