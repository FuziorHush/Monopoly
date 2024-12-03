using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.IO;
using System;

public class GamePropertiesLoader : MonoBehaviour
{
    private string _path;
    public static bool IsLoaded { get; private set; }

    private void Awake()
    {
        if (!IsLoaded)
        {
            _path = Application.streamingAssetsPath + "/gameProperties.json";
            gameObject.AddComponent<GamePropertiesController>().Init(LoadProperties());
            IsLoaded = true;
            Destroy(this);
        }
        else {
            Destroy(gameObject);
        }
    }

    public GameProperties LoadProperties()
    {
        if (File.Exists(_path))
        {
            try
            {
                string jsonText = File.ReadAllText(_path);
                GameProperties gameProperties = JsonUtility.FromJson<GameProperties>(jsonText);
                if (gameProperties.PropertiesVersion == Settings.PROPERTIES_VERSION)
                {
                    return gameProperties;
                }
                else 
                {
                    throw new Exception("gameProperties.json version don't match");
                }
            }
            catch (Exception) 
            {
                throw new Exception("gameProperties.json is invalid");
            }
        }
        else
        {
            throw new Exception("gameProperties.json is missing");
        }
    }
}
