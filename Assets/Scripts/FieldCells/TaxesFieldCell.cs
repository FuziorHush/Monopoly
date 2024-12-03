public class TaxesFieldCell : FieldCell
{
    private float _taxesAmount;

    public override void Init()
    {
        _taxesAmount = GamePropertiesController.GameProperties.TaxesAmount;
    }

    public override void Interact(Player player)
    {
        BalancesController.Instance.WidthdrawBalance(player, _taxesAmount);
    }

    protected override void ShowInfo()
    {
        InfoMenu.Instance.ShowInfoTaxes();
    }
}
