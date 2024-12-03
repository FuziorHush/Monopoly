using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RoomlistItem : MonoBehaviour
{
    private string _roomName;

    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _numPlayersText;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(JoinRoom);
    }

    public void Init(string name, int numPlayers) 
    {
        _roomName = name;
        _nameText.text = name;
        _numPlayersText.text = numPlayers + "/4";
    }

    private void JoinRoom()
    {
        CreateAndJoinRooms.Instance.JoinRoom(_roomName);
    }
}
