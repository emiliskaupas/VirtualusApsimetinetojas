using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class TalkingPage : MonoBehaviour
{
    [SerializeField] private Button BackButton;
    [SerializeField] private Button MeniuButton;
    [SerializeField] private Image Meniu;
    [SerializeField] private Button SettingsButton;
    [SerializeField] private Button AvatarChangeButton;
    [SerializeField] private Button EndSessionButton;
    [SerializeField] private Canvas SettingsCanvas;
    [SerializeField] private Button BacktoSessionButton;

    //idk ar veiks ar reiks
    [Header("Text References")]
    [SerializeField] private TMP_Text answerText;
    [SerializeField] private TMP_Text inputText;
    [SerializeField] private TMP_Text placeholderText;
    [SerializeField] private TMP_Text infoText;


    public static float DefaultAnswerSize = 20f;
    public static float DefaultInputSize = 16f;
    public static float DefaultPlaceholderSize = 14f;



    [Header("Text Size Settings")]
    [SerializeField] private float answerTextSize = 20f;
    [SerializeField] private float inputTextSize = 16f;
    [SerializeField] private float placeholderTextSize = 14f;
    [SerializeField] private float infoTextSize = 14f;
    //




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
        MeniuButton.onClick.AddListener(OnMeniuButton);
        SettingsButton.onClick.AddListener(OnSettinsButton);
        AvatarChangeButton.onClick.AddListener(OnAvatarChangeButton);
        EndSessionButton.onClick.AddListener(OnBackButton);
        BacktoSessionButton.onClick.AddListener(OnBackToSessionButton);
        //prideta eil
        //InitializeUI();
        InitializeTextSizes();


        Meniu.gameObject.SetActive(false);
        SettingsCanvas.gameObject.SetActive(false);
    }

    //pridetas metodas
    private void InitializeTextSizes()
    {
        /*
        if (answerText != null)
        {
            answerText.fontSize = DefaultAnswerSize;
            answerText.enableAutoSizing = false;
        }

        if (inputText != null)
        {
            inputText.fontSize = DefaultInputSize;
            inputText.enableAutoSizing = false;
        }

        if (placeholderText != null)
        {
            placeholderText.fontSize = DefaultPlaceholderSize;
            placeholderText.enableAutoSizing = false;
        }*/

        SetTextElement(answerText, answerTextSize);
        SetTextElement(inputText, inputTextSize);
        SetTextElement(placeholderText, placeholderTextSize);

        if (infoText == null)
        {
            infoText = GameObject.Find("InfoText")?.GetComponent<TMP_Text>();
        }
        // TMP_Text infoText = GameObject.Find("InfoText")?.GetComponent<TMP_Text>();
        SetTextElement(infoText, infoTextSize);
    }
    //virsuje pridetas metodas
    //private void InitializeUI()
    //{
    //    BackButton.onClick.AddListener(OnBackButton);
    //    MeniuButton.onClick.AddListener(OnMeniuButton);
    //    SettingsButton.onClick.AddListener(OnSettinsButton);
    //    AvatarChangeButton.onClick.AddListener(OnAvatarChangeButton);
    //    EndSessionButton.onClick.AddListener(OnBackButton);
    //    BacktoSessionButton.onClick.AddListener(OnBackToSessionButton);

    //    Meniu.gameObject.SetActive(false);
    //    SettingsCanvas.gameObject.SetActive(false);
    //}

    //pridetas
    private void SetTextElement(TMP_Text textElement, float size)
    {
        if (textElement != null)
        {
            textElement.fontSize = size;
            textElement.enableAutoSizing = false;
            textElement.ForceMeshUpdate();
            //prideta
            Debug.Log($"Set {textElement.name} size to {size}");
        }
    }
    //virsuje pridetas metodas





    private bool IsSceneLoaded(string sceneName)
    {
        Scene scene = SceneManager.GetSceneByName(sceneName);
        return scene.isLoaded;
    }

    [System.Obsolete]
    private void OnBackButton()
    {
        string lastScene = "";

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

        if (!string.IsNullOrEmpty(lastScene))
        {
            PlayerPrefs.SetString("LastTalkingPage", lastScene);
            PlayerPrefs.Save();
        }

        LoadScene("Pradinis langas");
        Debug.Log("Pradinis langas loaded");
    }
    private void OnMeniuButton()
    {
        //if meniubutton is pressed once the meniu will appear, if pressed again it will disappear
        bool isActive = !Meniu.gameObject.activeSelf;
        if (isActive) {
            Meniu.gameObject.SetActive(isActive);
        }
        else
        {
            Meniu.gameObject.SetActive(isActive);
        }
    }
    private void OnSettinsButton()
    {
       
            SettingsCanvas.gameObject.SetActive(true);
       
    }
    private void OnAvatarChangeButton()
    {
        string newScene = "";
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

        if (!string.IsNullOrEmpty(newScene))
        {
            PlayerPrefs.SetString("LastTalkingPage", newScene);
            PlayerPrefs.Save();
            LoadScene(newScene);
        }
    }

    private void OnBackToSessionButton()
    {
        SettingsCanvas.gameObject.SetActive(false);
    }
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

}
