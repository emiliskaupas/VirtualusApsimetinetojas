using UnityEngine;
using TMPro;
public class LabelText : MonoBehaviour
{
    public TMP_Dropdown dropdown;
    public TextMeshProUGUI[] text;
    public void ChangeTextSize()
    {
        if (dropdown.value == 0)
        {
            foreach (TextMeshProUGUI t in text)
            {
                if (t.name == "AnswerText")
                {
                    t.fontSize = 80;
                }
                else if (t.name == "InputText")
                {
                    t.fontSize = 30;
                }
                else
                    t.fontSize = 30;
            }
        }
        else if (dropdown.value == 1)
        {
            foreach (TextMeshProUGUI t in text)
            {
                if (t.name == "AnswerText")
                {
                    t.fontSize = 50;
                }
                else if (t.name == "InputText")
                {
                    t.fontSize = 10;
                }
                else
                    t.fontSize = 20;
            }
        }
        else if (dropdown.value == 2)
        {
            foreach (TextMeshProUGUI t in text)
            {
                if (t.name == "AnswerText")
                {
                    t.fontSize = 30;
                }
                else if (t.name == "InputText")
                {
                    t.fontSize = 7;
                }
                else
                    t.fontSize = 15;
            }
        }
    }
}
