using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public Player(string name, int number, Transform avatarTransform, AvatarColor avatarColor)
    {
        Name = name;
        Number = number;
        EstatesOwn = new List<Estate>();
        AvatarTransform = avatarTransform;
        AvatarColor = avatarColor;
        AvatarColorHex = ColorUtility.ToHtmlStringRGBA(avatarColor.FrontColor);
    }

    public string Name { get; private set; }
    public int Number { get; private set; }
    public AvatarColor AvatarColor { get; private set; }
    public string AvatarColorHex { get; private set; }

    private float _balance;
    public float Balance {
        get => _balance;
        set {
            _balance = value;

            if (_balance < 0)
                GameEvents.PlayerBalanceIsNegative?.Invoke(this, value);
        }
    }

    public int GOJHas { get; set; }
    public FieldCell CellOn { get; set; }
    public int Loops { get; set; }
    public Team Team { get; set; }
    public List<Estate> EstatesOwn { get; private set; }
    public Transform AvatarTransform { get; private set; }

    public Photon.Realtime.Player NetworkPlayer { get; set; }
}
