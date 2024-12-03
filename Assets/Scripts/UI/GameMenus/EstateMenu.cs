using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class EstateMenu : MonoSingleton<EstateMenu>
{
    [SerializeField] private Button[] _buyButtons;
    [SerializeField] private TMP_Text[] _buyButtonsTexts;
    [SerializeField] private GameObject _windowGameObject;
    [SerializeField] private TMP_Text _estateName;

    [SerializeField] private float _windowOffset;
    [SerializeField] private float _windowAnimationTime;

    public bool MenuIsOpen { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        for (int i = 1; i < 6; i++)
        {
            int cl = i;
            _buyButtons[i-1].onClick.AddListener(delegate {EstatesController.Instance.Purchase(cl); });
        }
        _windowGameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(_windowOffset, 0, 0);

        GameEvents.NewTurn += OnNewTurn;
        GameEvents.DicesTossed += OnDicesTossed;
    }

    public void Init() 
    {
        _windowGameObject.SetActive(false);
    }

    public void Open(Player player, Estate estate)
    {
        _windowGameObject.transform.DOLocalMoveX(0, _windowAnimationTime);

        float[] prices = new float[5];
        float quantity = estate.BaseQuantity;

        _estateName.text = estate.Name;
        for (int i = 0; i < estate.Level; i++)
        {
            _buyButtons[i].interactable = false;
            _buyButtonsTexts[i].text = LanguageSystem.Instance["estatesmenu_bought"];
            prices[i] = 0;
        }
        for (int i = estate.Level; i < 5; i++)
        {
            _buyButtons[i].interactable = player.Balance >= quantity;

            _buyButtonsTexts[i].text = quantity.ToString() + "$";
            prices[i] = quantity;
            quantity += estate.BaseQuantity;
        }

        EstatesController.Instance.SetTarget(player, estate);
        EstatesController.Instance.SetPrices(prices);
        MenuIsOpen = true;
    }

    private void OnNewTurn(Player player) 
    {
        CloseMenu();
    }

    private void OnDicesTossed(Vector2Int result)
    {
        CloseMenu();
    }

    public void CloseMenu() 
    {
        for (int i = 0; i < _buyButtons.Length; i++)
        {
            _buyButtons[i].interactable = false;
        }
        _windowGameObject.transform.DOLocalMoveX(_windowOffset, _windowAnimationTime);

        MenuIsOpen = false;
    }
}
