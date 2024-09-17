using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public Player(string name, int number, Transform avatarTransform)
    {
        Name = name;
        Number = number;
        EstatesOwn = new List<Estate>();
        AvatarTransform = avatarTransform;
    }

    public string Name { get; private set; }
    public int Number { get; private set; }

    private float _balance;
    public float Balance {
        get => _balance;
        set {
            _balance = value;
            GameEvents.PlayerBalanceChanged?.Invoke(this,  value, _balance);

            if (_balance < 0)
                GameEvents.PlayerBalanceIsNegative?.Invoke(this, value);
        }
    }

    public int GOJHas { get; set; }
    public int CellOn { get; set; }
    public int Loops { get; set; }
    public Team Team { get; set; }
    public List<Estate> EstatesOwn { get; private set; }
    public Transform AvatarTransform { get; private set; }

    public Photon.Realtime.Player User { get; set; }
}
