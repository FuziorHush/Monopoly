using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Events;
using System.Text;

public class LanguageSystem : MonoSingleton<LanguageSystem>
{
    private Dictionary<string, string> LocalizedText;
    public bool IsDone { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
        LoadLanguage("ru_RU");
    }

    public bool LoadLanguage(string lang) {

        if (!ReadFromFile(Application.streamingAssetsPath + "/Languages/" + lang + ".json", ref LocalizedText))
            return false;

        IsDone = true;
        GameEvents.LanguageChanged?.Invoke();
        return true;
    }

    private bool ReadFromFile(string path, ref Dictionary<string, string> dictionary) {
        if (File.Exists(path))
        {
            string jsonText = File.ReadAllText(path);
            LanguageData loadedData = JsonUtility.FromJson<LanguageData>(jsonText);
            dictionary = new Dictionary<string, string>();
            for (int i = 0; i < loadedData.items.Length; i++)
            {
                dictionary.Add(loadedData.items[i].key, loadedData.items[i].value);
            }

            return true;
        }
        else
        {
            throw new System.Exception("Language file is missing");
        }
    }

    public string this[string key]
    {
        get
        {
            if (string.IsNullOrEmpty(key))
                return string.Empty;

            if (LocalizedText.ContainsKey(key))
            {
                return LocalizedText[key];
            }
            else {
                return key;
            }
        }
    }
}

[System.Serializable]
public class LanguageData {
    public LanguageItem[] items;
}

[System.Serializable]
public class LanguageItem {
    public string key;
    public string value;
}
