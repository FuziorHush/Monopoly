public class BankControllerLocal : BankController
{
    public override void HandOverMoney(Player playerSender, Player playerReciever, float amount)
    {
        if (playerSender.Balance >= amount)
        {
            BalancesController.Instance.Transferbalance(playerSender, playerReciever, amount);
        }
    }
}
