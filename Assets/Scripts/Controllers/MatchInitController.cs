using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class MatchInitController : MonoBehaviourPunCallbacks
{
    [SerializeField] private Transform _controllersParent;

    private void Start()
    {
        if (MultiplayerGameTypeController.CurrentType == MultiplayerGameType.Local)
        {
            AddController<GameFlowControllerLocal>("GameFlowController");
            AddController<FieldControllerLocal>("FieldController");
            AddController<JailControllerLocal>("JailController");
            AddController<BalancesControllerLocal>("BalancesController");
            AddController<BankControllerLocal>("BankController");
            AddController<DecksControllerLocal>("DecksController");
            AddController<EstatesControllerLocal>("EstatesController");
            AddController<OlympControllerLocal>("OlympController");

            GameEvents.ControllersCreated();

            GameFlowController.Instance.CreatePlayers();
            GameFlowController.Instance.StartMatch();
        }

        else if (MultiplayerGameTypeController.CurrentType == MultiplayerGameType.Photon)
        {
            AddController<GameFlowControllerPhoton>("GameFlowController");
            AddController<FieldControllerPhoton>("FieldController");
            AddController<JailControllerPhoton>("JailController");
            AddController<BalancesControllerPhoton>("BalancesController");
            AddController<BankControllerPhoton>("BankController");
            AddController<DecksControllerPhoton>("DecksController");
            AddController<EstatesControllerPhoton>("EstatesController");
            AddController<OlympControllerPhoton>("OlympController");

            GameEvents.ControllersCreated();

            if (PhotonNetwork.IsMasterClient) 
            {
                GameFlowController.Instance.CreatePlayers();
                GameFlowController.Instance.StartMatch();
            }
        }
    }

    private void AddController<T>(string name) where T : MonoBehaviour
    {
        GameObject controller = new GameObject(name);
        controller.transform.SetParent(_controllersParent);
        controller.AddComponent<T>();
    }
}
