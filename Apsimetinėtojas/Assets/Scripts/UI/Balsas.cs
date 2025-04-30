using UnityEngine;
using TMPro;

public class Balsas : MonoBehaviour
{
    public TMP_Dropdown dropdown;
    public string voice1 = "nova";
    public string voice2 = "echo";

    public void ChangeVoice()
    {
        WhisperAPI whisperAPI = FindObjectOfType<WhisperAPI>();

        if (whisperAPI == null)
        {
            Debug.LogError("WhisperAPI script not found in the scene!");
            return;
        }
        if (dropdown.value == 0)
        {
            whisperAPI.selectedVoice = voice1;
            Debug.Log("Voice changed to: " + voice1);
        }
        else if (dropdown.value == 1)
        {
            whisperAPI.selectedVoice = voice2;
            Debug.Log("Voice changed to: " + voice2);
        }
    }
}
