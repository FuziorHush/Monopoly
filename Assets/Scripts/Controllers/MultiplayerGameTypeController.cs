public static class MultiplayerGameTypeController
{
    public static MultiplayerGameType CurrentType = MultiplayerGameType.NotStarted;
}

public enum MultiplayerGameType
{
    NotStarted,
    Local,
    Photon
}
