using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KeyboardAwareUI : MonoBehaviour
{
    public RectTransform panelToMove; 
    private Vector3 originalPosition;
    private float keyboardOffset = 625f; 

    void Start()
    {
        if (panelToMove == null)
            panelToMove = GetComponent<RectTransform>();

        originalPosition = panelToMove.localPosition;
    }

    void Update()
    {
#if UNITY_ANDROID && !UNITY_EDITOR

        if (TouchScreenKeyboard.visible)
        {
            panelToMove.localPosition = Vector3.Lerp(panelToMove.localPosition,
                                                     originalPosition + new Vector3(0, keyboardOffset, 0),
                                                     Time.deltaTime * 5);
        }
        else
        {
            panelToMove.localPosition = Vector3.Lerp(panelToMove.localPosition,
                                                     originalPosition,
                                                     Time.deltaTime * 5);
        }
#endif
    }
}
