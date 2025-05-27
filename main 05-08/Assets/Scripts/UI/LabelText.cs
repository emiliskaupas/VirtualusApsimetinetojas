
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LabelText : MonoBehaviour
{
    public TMP_Dropdown dropdown;
    public TextMeshProUGUI[] allTextElements;
    public static float currentFontSizeMultiplier = 1f;

    //pridetas sudas
    [SerializeField] private float answerBaseSize = 20f;
    [SerializeField] private float inputBaseSize = 16f;
    [SerializeField] private float otherBaseSize = 14f;

    private void Start()
    {

        ApplyFontSizeToAllElements();

        //pridetas sudas
        TalkingPage talkingPage = FindObjectOfType<TalkingPage>();
        if (talkingPage != null)
        {
            answerBaseSize = TalkingPage.DefaultAnswerSize;
            inputBaseSize = TalkingPage.DefaultInputSize;
            otherBaseSize = TalkingPage.DefaultPlaceholderSize;
        }

        LoadFontSettings();
        ApplyFontSizeToAllElements();

        if (dropdown != null)
        {
            dropdown.onValueChanged.AddListener(ChangeTextSize);
            dropdown.value = GetDropdownValueBasedOnCurrentSize();
        }
        //virsuj pridetas sudas

        if (dropdown != null)
        {
            dropdown.onValueChanged.AddListener(ChangeTextSize);
            dropdown.value = GetDropdownValueBasedOnCurrentSize();
        }
    }

    public void ChangeTextSize(int value)
    {
        switch (value)
        {
            case 0:
                currentFontSizeMultiplier = 1.5f;
                break;
            case 1:
                currentFontSizeMultiplier = 1f;
                break;
            case 2:
                currentFontSizeMultiplier = 0.7f;
                break;
        }

        ApplyFontSizeToAllElements();
        SaveFontSizeSetting();
    }

    private void ApplyFontSizeToAllElements()
    {

        TextMeshProUGUI[] allTexts = Resources.FindObjectsOfTypeAll<TextMeshProUGUI>();

        foreach (TextMeshProUGUI text in allTexts)
        {
            if (text.name == "AnswerText")
            {
                text.fontSize = 40 * currentFontSizeMultiplier;
            }
            else if (text.name == "InputText")
            {
                text.fontSize = 20 * currentFontSizeMultiplier;
            }
            else
            {
                text.fontSize = 20 * currentFontSizeMultiplier;
            }
        }
    }

    private int GetDropdownValueBasedOnCurrentSize()
    {
        if (Mathf.Approximately(currentFontSizeMultiplier, 1.5f)) return 0;
        if (Mathf.Approximately(currentFontSizeMultiplier, 1f)) return 1;
        return 2;
    }

    private void SaveFontSizeSetting()
    {
        PlayerPrefs.SetFloat("FontSizeMultiplier", currentFontSizeMultiplier);
        PlayerPrefs.Save();
    }

    public static void LoadFontSettings()
    {
        if (PlayerPrefs.HasKey("FontSizeMultiplier"))
        {
            currentFontSizeMultiplier = PlayerPrefs.GetFloat("FontSizeMultiplier");
        }
        else
        {
            currentFontSizeMultiplier = 1f;
        }
    }
}
