using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomlistItem : MonoBehaviour
{
    public string RoomName;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(JoinRoom);
    }

    private void JoinRoom()
    {
        CreateAndJoinRooms.Instance.JoinRoom(RoomName);
    }
}
