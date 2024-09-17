using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    public static LobbyManager Instance;

    [SerializeField] private GameObject _browseMenu;
    [SerializeField] private GameObject _roomMenu;

    private void Awake()
    {
        Instance = this;
    }

    public void SwitchToRoomMenu() {
        _browseMenu.SetActive(false);
        _roomMenu.SetActive(true);

    }

    public void SwitchToBrowseMenu()
    {
        _roomMenu.SetActive(false);
        _browseMenu.SetActive(true);
    }
}
