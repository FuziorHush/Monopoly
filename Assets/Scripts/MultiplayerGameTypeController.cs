using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MultiplayerGameTypeController
{
    public static MultiplayerGameType CurrentType = MultiplayerGameType.NotStarted;
}

public enum MultiplayerGameType { 
NotStarted,
Local,
Photon
}
