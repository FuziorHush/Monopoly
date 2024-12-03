public class BalancesControllerLocal : BalancesController
{
    public override void AddBalance(Player player, float amount)
    {
        player.Balance += amount;
        GameEvents.PlayerBalanceAdded?.Invoke(player, amount);
    }

    public override void WidthdrawBalance(Player player, float amount)
    {
        player.Balance -= amount;
        GameEvents.PlayerBalanceWidthdrawn?.Invoke(player, amount);
    }

    public override void Transferbalance(Player playerSender, Player playerReceiver, float amount)
    {
        playerSender.Balance -= amount;
        playerReceiver.Balance += amount;
        GameEvents.BalanceTransfered?.Invoke(playerSender, playerReceiver, amount);
    }
}
