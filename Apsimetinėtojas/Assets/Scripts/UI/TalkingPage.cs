using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TalkingPage : MonoBehaviour
{
    [SerializeField] private Button BackButton;
    [SerializeField] private Button OptionButton;
    [SerializeField] private Button MicrophoneButton;
    [SerializeField] private Button MicrophoneCancelButton;
    [SerializeField] private Button EndSessionButton;

    [SerializeField] private Button Avatar1;
    [SerializeField] private Button Avatar2;
    [SerializeField] private Button Avatar3;

    [SerializeField] private SpriteRenderer CharacterRenderer;
    [SerializeField] private Sprite Avatar1Sprite;
    [SerializeField] private Sprite Avatar2Sprite;
    [SerializeField] private Sprite Avatar3Sprite;

    private bool areButtonsVisible = false;
    private Button selectedAvatar = null; // Store the selected avatar
    private SpriteRenderer spriteRenderer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [System.Obsolete]
    void Start()
    {
        if (IsSceneLoaded("Pradinis langas"))
        {
            SceneManager.UnloadScene("Pradinis langas");
            Debug.Log("Bendravimo langas loaded");
        }
        if (IsSceneLoaded("results"))
        {
            SceneManager.UnloadScene("results");
            Debug.Log("Bendravimo langas loaded");
        }
        BackButton.onClick.AddListener(OnBackButton);
        OptionButton.onClick.AddListener(OnOptionsButton);
        EndSessionButton.onClick.AddListener(OnSessionEnd);

        MicrophoneButton.onClick.AddListener(ToggleMicrophoneButtons);
        MicrophoneCancelButton.onClick.AddListener(ToggleMicrophoneButtons);
        MicrophoneCancelButton.gameObject.SetActive(false);
        MicrophoneButton.gameObject.SetActive(true);

        // Set initial sprite and avatar positions
        CharacterRenderer.sprite = Avatar2Sprite;

        selectedAvatar = Avatar2;
        // Initial Avatar setup
        Avatar1.onClick.AddListener(() => SelectAvatar(Avatar1));
        Avatar2.onClick.AddListener(() => SelectAvatar(Avatar2));
        Avatar3.onClick.AddListener(() => SelectAvatar(Avatar3));

        // Set avatars active or not based on your logic
        Avatar1.gameObject.SetActive(false);
        Avatar2.gameObject.SetActive(false);
        Avatar3.gameObject.SetActive(false);

    }

    private bool IsSceneLoaded(string sceneName)
    {
        Scene scene = SceneManager.GetSceneByName(sceneName);
        return scene.isLoaded;
    }
    private void ToggleMicrophoneButtons()
    {
        bool isMicCancelActive = MicrophoneCancelButton.gameObject.activeSelf;
        MicrophoneCancelButton.gameObject.SetActive(!isMicCancelActive);
        MicrophoneButton.gameObject.SetActive(isMicCancelActive);
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
        areButtonsVisible = !areButtonsVisible; // Toggle visibility state

        // Show or hide avatars based on the toggle state
        Avatar1.gameObject.SetActive(areButtonsVisible);
        Avatar2.gameObject.SetActive(areButtonsVisible);
        Avatar3.gameObject.SetActive(areButtonsVisible);
    }

    private void SelectAvatar(Button avatar)
    {
        selectedAvatar = avatar; // Store selected avatar
        avatar.gameObject.SetActive(false); // Hide selected avatar button

        // Reposition the remaining avatars dynamically
        RepositionAvatars();
    }

    private void RepositionAvatars()
    {
        // Reset positions and ensure avatars are shown in the right places based on the selection
        if (selectedAvatar == Avatar1)
        {
            Avatar2.transform.localPosition = new Vector3(-110, 834, 1);
            Avatar3.transform.localPosition = new Vector3(135, 834, 1);
            Avatar2.gameObject.SetActive(true); // Show other avatars
            Avatar3.gameObject.SetActive(true);
            CharacterRenderer.sprite = Avatar1Sprite;
        }
        else if (selectedAvatar == Avatar2)
        {
            Avatar1.transform.localPosition = new Vector3(-110, 834, 1);
            Avatar3.transform.localPosition = new Vector3(135, 834, 1);
            Avatar1.gameObject.SetActive(true);
            Avatar3.gameObject.SetActive(true);
            CharacterRenderer.sprite = Avatar2Sprite;
        }
        else if (selectedAvatar == Avatar3)
        {
            Avatar1.transform.localPosition = new Vector3(-110, 834, 1);
            Avatar2.transform.localPosition = new Vector3(135, 834, 1);
            Avatar1.gameObject.SetActive(true);
            Avatar2.gameObject.SetActive(true);
            CharacterRenderer.sprite = Avatar3Sprite;
        }
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    [System.Obsolete]
    private void OnSessionEnd()
    {
        if (IsSceneLoaded("Bendravimo langas"))
        {
            SceneManager.UnloadScene("Bendravimo langas");
            LoadScene("results");
            Debug.Log("results langas loaded");
        }
    }

}
