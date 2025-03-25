using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartPage : MonoBehaviour
{
    // UI elements assigned from the Unity Editor
    [SerializeField] private Button StartButton;
    [SerializeField] private Button InformationButton;
    [SerializeField] private Button BackButton;
    [SerializeField] private Button OptionButton;

    [SerializeField] private GameObject infoScrollView; // UI ScrollView for displaying information
    [SerializeField] private TMP_Text infoText; // Text element to display information content

    /// <summary>
    /// Called once before the first execution of Update after the MonoBehaviour is created.
    /// Used for initialization.
    /// </summary>
    [System.Obsolete]
    void Start()
    {
        // Unload any previously loaded talking scenes to reset the state
        if (IsSceneLoaded("Bendravimo langas female"))
        {
            SceneManager.UnloadScene("Bendravimo langas female");
            Debug.Log("Pradinis langas loaded");
        }
        else if (IsSceneLoaded("Bendravimo langas male"))
        {
            SceneManager.UnloadScene("Bendravimo langas male");
            Debug.Log("Pradinis langas loaded");
        }

        // Assign button click event listeners to corresponding methods
        StartButton.onClick.AddListener(onStartButton);
        InformationButton.onClick.AddListener(ToggleInformation);

        // Ensure UI elements are hidden initially
        infoScrollView.SetActive(false);
        BackButton.gameObject.SetActive(false);
        OptionButton.gameObject.SetActive(false);
    }

    /// <summary>
    /// Toggles the visibility of the information scroll view.
    /// Displays text content when activated.
    /// </summary>
    private void ToggleInformation()
    {
        // Toggle visibility of the info panel
        bool isActive = !infoScrollView.activeSelf;
        infoScrollView.SetActive(isActive);

        // Set text content when visible
        if (isActive)
        {
            infoText.text = "Lorem ipsum dolor sit amet, consectetur adipisicing elit. Odit dolor at ea tempora " +
                            "voluptatibus dignissimos nesciunt totam, nostrum beatae sapiente sed recusandae rem voluptates " +
                            "suscipit tenetur est cupiditate sint excepturi?" +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit. Odit dolor at ea tempora voluptatibus dignissimos " +
                            "nesciunt totam, nostrum beatae sapiente sed recusandae rem voluptates suscipit tenetur est cupiditate sint excepturi?";
        }
    }

    /// <summary>
    /// Loads a scene based on the given scene name.
    /// </summary>
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
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
    /// Handles the Start button click event.
    /// Unloads the current scene and loads the last used talking page or a default scene.
    /// </summary>
    [System.Obsolete]
    private void onStartButton()
    {
        // Check if the "Pradinis langas" scene is loaded and unload it before starting a new scene
        if (IsSceneLoaded("Pradinis langas"))
        {
            SceneManager.UnloadSceneAsync("Pradinis langas");

            // Retrieve the last used scene from PlayerPrefs or default to "Bendravimo langas female"
            string lastScene = PlayerPrefs.GetString("LastTalkingPage", "Bendravimo langas female");

            // Load the last active scene
            LoadScene(lastScene);
            Debug.Log($"{lastScene} loaded");
        }
    }

}
