using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TalkingPage : MonoBehaviour
{
    [SerializeField] private Button BackButton;
    [SerializeField] private Button OptionButton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [System.Obsolete]
    void Start()
    {
        if (IsSceneLoaded("Pradinis langas"))
        {
            SceneManager.UnloadScene("Pradinis langas");
            Debug.Log("Bendravimo langas loaded");
        }
        BackButton.onClick.AddListener(OnBackButton);
        OptionButton.onClick.AddListener(OnOptionsButton);
    }

    private bool IsSceneLoaded(string sceneName)
    {
        Scene scene = SceneManager.GetSceneByName(sceneName);
        return scene.isLoaded;
    }

    [System.Obsolete]
    private void OnBackButton()
    {
        if (IsSceneLoaded("Bendravimo langas"))
        {
            SceneManager.UnloadScene("Bendravimo langas");
            LoadScene("Pradinis langas");
            Debug.Log("Pradinis langas loaded");
        }
    }
    private void OnOptionsButton()
    {

    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

}
