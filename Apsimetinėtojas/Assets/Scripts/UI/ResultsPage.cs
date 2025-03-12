using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultsPage : MonoBehaviour
{
    [SerializeField] private Button StartButton;
    [SerializeField] private Button InformationButton;
    [SerializeField] private Button BackButton;
    [SerializeField] private Button OptionButton;

    [SerializeField] private GameObject infoScrollView;
    [SerializeField] private TMP_Text infoText;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [System.Obsolete]
    void Start()
    {
        if (IsSceneLoaded("Bendravimo langas"))
        {
            SceneManager.UnloadScene("Bendravimo langas");
            //LoadScene("Pradinis langas");
            Debug.Log("Rezultatu langas loaded");
        }
        if(IsSceneLoaded("Pradinis langas"))
        {
            SceneManager.UnloadScene("Pradinis langas");
            //LoadScene("Pradinis langas");
            Debug.Log("Rezultatu langas loaded");
        }
        StartButton.onClick.AddListener(onStartButton);
        InformationButton.onClick.AddListener(ToggleInformation);
        // Ensure the text is hidden at the start
        infoScrollView.SetActive(false);
        BackButton.gameObject.SetActive(false);
        OptionButton.gameObject.SetActive(false);
    }
    private void ToggleInformation()
    {
        // Toggle visibility of Scroll View
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

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    private bool IsSceneLoaded(string sceneName)
    {
        Scene scene = SceneManager.GetSceneByName(sceneName);
        return scene.isLoaded;
    }

    [System.Obsolete]
    private void onStartButton()
    {
        if (IsSceneLoaded("results"))
        {
            SceneManager.UnloadScene("results");
            LoadScene("Bendravimo langas");
            Debug.Log("Bendravimo langas loaded");
        }
    }
}
