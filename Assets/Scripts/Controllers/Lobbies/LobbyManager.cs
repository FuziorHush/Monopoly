using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    public static LobbyManager Instance;

    [SerializeField] private GameObject _browseMenu;
    [SerializeField] private GameObject _roomMenu;
    [SerializeField] private GameObject _localMenu;
    [SerializeField] private LocalRoomManager _localRoom;

    private void Awake()
    {
        Instance = this;
    }

    public void SwitchToRoomMenu() 
    {
        _browseMenu.SetActive(false);
        _roomMenu.SetActive(true);
    }

    public void SwitchToBrowseMenu()
    {
        _roomMenu.SetActive(false);
        _localMenu.SetActive(false);
        _browseMenu.SetActive(true);
    }

    public void SwitchToLocalMenu()
    {
        _browseMenu.SetActive(false);
        _localMenu.SetActive(true);
        _localRoom.OpenInit();
    }
}
