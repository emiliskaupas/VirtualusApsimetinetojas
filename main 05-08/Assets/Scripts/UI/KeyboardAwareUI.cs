using UnityEngine;
using UnityEngine.UI;

public class KeyboardAwareUI : MonoBehaviour
{
    public RectTransform inputFieldContainer; 
    public float offset = 10f;
    private Vector3 originalPosition;

    void Start()
    {
        originalPosition = inputFieldContainer.localPosition;
    }

    void Update()
    {
#if UNITY_IOS || UNITY_ANDROID
        if (TouchScreenKeyboard.visible)
        {
            Rect keyboardRect = TouchScreenKeyboard.area;
            if (keyboardRect.height > 0)
            {
                // Convert keyboard height to UI canvas space
                float keyboardHeight = keyboardRect.height / Screen.height * GetComponentInParent<Canvas>().GetComponent<RectTransform>().rect.height;
                Vector3 newPosition = originalPosition + new Vector3(0, keyboardHeight + offset, 0);
                inputFieldContainer.localPosition = newPosition;
            }
        }
        else
        {
            inputFieldContainer.localPosition = originalPosition;
        }
#endif
    }
}
