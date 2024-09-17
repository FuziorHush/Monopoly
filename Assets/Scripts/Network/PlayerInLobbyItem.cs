using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInLobbyItem : MonoBehaviour
{
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Dropdown _teamDropdown;

    public void SetName(string nickname) 
    {
        _nameText.text = nickname;
    }
}
