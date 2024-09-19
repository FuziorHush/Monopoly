using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAvatar : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRendererFront;
    [SerializeField] private SpriteRenderer _spriteRendererBack;

    void Start()
    {
        //SpriteRenderer.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
    }
}
