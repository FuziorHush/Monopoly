using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.IO;

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
            string jsonText = File.ReadAllText(_path);
            return JsonUtility.FromJson<GameProperties>(jsonText);
        }
        else
        {
            throw new System.Exception("gameProperties.json is missing");
        }
    }
}
