using OpenAI;
using System.IO;
using UnityEngine;
using static UnityEngine.Rendering.STP;

// A simple data structure to hold configuration values loaded from a JSON file.
[SerializeField]
public class ConfigData
{
    // API Key for authenticating requests to OpenAI
    public string api_key;

    // The model to use in OpenAI's API
    public string model;

    // The Whisper model to use for speech recognition
    public string whisper;

    // The language setting, used to set the language for transcription and possibly other services
    public string language;

    // A prompt or instruction that will be used as a context for OpenAI models
    public string prompt;
}

public class ConfigLoader : MonoBehaviour
{
    // Static instance of the configuration that will be accessible throughout the application
    public static ConfigData config;

    // Called when the script is initialized
    void Awake()
    {
        // Load the configuration settings when the game starts
        LoadConfig();
    }

    /// <summary>
    /// This method loads the configuration data from a JSON file located at a specified path.
    /// </summary>
    private void LoadConfig()
    {
        // Path to the configuration JSON file
        string path = "Assets\\Scripts\\OpenAI Unity\\0.2.2\\ChatGPT\\config.json";

        // Check if the configuration file exists at the specified path
        if (File.Exists(path))
        {
            // Read the JSON content from the file
            string json = File.ReadAllText(path);

            // Deserialize the JSON content into the ConfigData object
            config = JsonUtility.FromJson<ConfigData>(json);

            // Log success message after loading the config
            Debug.Log("Config loaded successfully!");
        }
        else
        {
            // Log an error message if the config file was not found
            Debug.LogError("Config file not found at: " + path);
        }
    }
}
