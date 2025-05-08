using OpenAI;
using System.IO;
using UnityEngine;
using static UnityEngine.Rendering.STP;

[SerializeField]
public class ConfigData
{
    public string api_key;
    public string model;
    public string prompt;
}

public class ConfigLoader : MonoBehaviour
{
    public static ConfigData config;

    void Awake()
    {
        LoadConfig();
    }

    private void LoadConfig()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("config"); // No ".json" extension needed

        if (jsonFile != null)
        {
            config = JsonUtility.FromJson<ConfigData>(jsonFile.text);
            Debug.Log("Config loaded successfully!");
        }
        else
        {
            Debug.LogError("Config file not found in Resources folder.");
        }
    }
}
