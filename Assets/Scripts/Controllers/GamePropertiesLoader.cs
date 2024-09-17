using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.IO;

public class GamePropertiesLoader : MonoBehaviour
{
    private string _path;

    private void Awake()
    {
        _path = Application.streamingAssetsPath + "/gameProperties.json";
        LoadProperties();
    }

    public GameProperties LoadProperties()
    {
        if (File.Exists(_path))
        {
            string jsonText = File.ReadAllText(_path);
            return JsonUtility.FromJson<GameProperties>(jsonText);
        }
        else
        {
            throw new System.Exception("gameProperties.json is missing");
        }
    }
}
