using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TalkingPage : MonoBehaviour
{
    // UI elements assigned from the Unity Editor
    [SerializeField] private Button BackButton;
    [SerializeField] private Button MeniuButton;
    [SerializeField] private Image Meniu;
    [SerializeField] private Button SettingsButton;
    [SerializeField] private Button AvatarChangeButton;
    [SerializeField] private Button EndSessionButton;
    [SerializeField] private Canvas SettingsCanvas;
    [SerializeField] private Button BacktoSessionButton;

    // Called once when the script is initialized
    [System.Obsolete]
    void Start()
    {
        // Unload the previous scene if it is loaded
        if (IsSceneLoaded("Pradinis langas"))
        {
            SceneManager.UnloadScene("Pradinis langas");
            Debug.Log("Bendravimo langas loaded");
        }

        // Assign button click listeners to respective methods
        BackButton.onClick.AddListener(OnBackButton);
        MeniuButton.onClick.AddListener(OnMeniuButton);
        SettingsButton.onClick.AddListener(OnSettinsButton);
        AvatarChangeButton.onClick.AddListener(OnAvatarChangeButton);
        EndSessionButton.onClick.AddListener(OnBackButton);
        BacktoSessionButton.onClick.AddListener(OnBackToSessionButton);

        // Hide the menu and settings canvas initially
        Meniu.gameObject.SetActive(false);
        SettingsCanvas.gameObject.SetActive(false);
    }

    /// <summary>
    /// Checks if a given scene is currently loaded.
    /// </summary>
    private bool IsSceneLoaded(string sceneName)
    {
        Scene scene = SceneManager.GetSceneByName(sceneName);
        return scene.isLoaded;
    }

    /// <summary>
    /// Handles the "Back" button click event.
    /// Unloads the current scene and navigates back to "Pradinis langas".
    /// Stores the last talking page in PlayerPrefs.
    /// </summary>
    [System.Obsolete]
    private void OnBackButton()
    {
        string lastScene = "";

        // Check which scene is currently loaded and unload it
        if (IsSceneLoaded("Bendravimo langas female"))
        {
            lastScene = "Bendravimo langas female";
            SceneManager.UnloadSceneAsync("Bendravimo langas female");
        }
        else if (IsSceneLoaded("Bendravimo langas male"))
        {
            lastScene = "Bendravimo langas male";
            SceneManager.UnloadSceneAsync("Bendravimo langas male");
        }

        // Save the last visited scene before going back
        if (!string.IsNullOrEmpty(lastScene))
        {
            PlayerPrefs.SetString("LastTalkingPage", lastScene);
            PlayerPrefs.Save();
        }

        // Load the starting scene
        LoadScene("Pradinis langas");
        Debug.Log("Pradinis langas loaded");
    }

    /// <summary>
    /// Toggles the visibility of the menu UI.
    /// </summary>
    private void OnMeniuButton()
    {
        // Toggle the menu visibility
        Meniu.gameObject.SetActive(!Meniu.gameObject.activeSelf);
    }

    /// <summary>
    /// Shows the settings canvas when the settings button is clicked.
    /// </summary>
    private void OnSettinsButton()
    {
        SettingsCanvas.gameObject.SetActive(true);
    }

    /// <summary>
    /// Handles the avatar change button click.
    /// Switches between "Bendravimo langas male" and "Bendravimo langas female" scenes.
    /// Saves the last active scene to PlayerPrefs.
    /// </summary>
    private void OnAvatarChangeButton()
    {
        string newScene = "";

        // Check the currently loaded scene and switch to the other
        if (IsSceneLoaded("Bendravimo langas male"))
        {
            SceneManager.UnloadSceneAsync("Bendravimo langas male");
            newScene = "Bendravimo langas female";
        }
        else if (IsSceneLoaded("Bendravimo langas female"))
        {
            SceneManager.UnloadSceneAsync("Bendravimo langas female");
            newScene = "Bendravimo langas male";
        }

        // Save the new scene choice and load it
        if (!string.IsNullOrEmpty(newScene))
        {
            PlayerPrefs.SetString("LastTalkingPage", newScene);
            PlayerPrefs.Save();
            LoadScene(newScene);
        }
    }

    /// <summary>
    /// Hides the settings canvas when returning to the session.
    /// </summary>
    private void OnBackToSessionButton()
    {
        SettingsCanvas.gameObject.SetActive(false);
    }

    /// <summary>
    /// Loads a new scene by name.
    /// </summary>
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
