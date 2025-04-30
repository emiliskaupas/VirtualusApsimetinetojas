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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        LoadConfig();
    }

    private void LoadConfig()
    {
        string path = "Assets\\Scripts\\OpenAI Unity\\0.2.2\\ChatGPT\\config.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            config = JsonUtility.FromJson<ConfigData>(json);
            Debug.Log("Config loaded successfully!");
        }
        else
        {
            Debug.LogError("Config file not found at: " + path);
        }
    }
}
