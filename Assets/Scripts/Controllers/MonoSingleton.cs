using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoSingleton<TType> : MonoBehaviour where TType : MonoSingleton<TType>
{
    private static TType _instance;
    public static TType Instance => _instance;

    protected virtual void Awake()
    {
        if (_instance != null)
        {
            Destroy(this);
            Debug.LogError($"Singleton {_instance.ToString()} is duplicated");
        }
        else
        {
            _instance = (TType)this;
        }
    }

    protected virtual void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
    }
}
