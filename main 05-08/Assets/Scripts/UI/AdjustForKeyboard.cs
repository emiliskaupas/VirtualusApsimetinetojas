using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class AdjustForKeyboard : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public RectTransform panelToMove; // The parent panel (e.g., a ScrollView)
    private Vector2 originalPosition;
    private bool isSelected;

    void Start()
    {
        originalPosition = panelToMove.anchoredPosition;
    }

    void Update()
    {
        if (isSelected && TouchScreenKeyboard.visible)
        {
            float keyboardHeight = TouchScreenKeyboard.area.height;
            panelToMove.anchoredPosition = new Vector2(originalPosition.x, keyboardHeight);
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        isSelected = true;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        isSelected = false;
        panelToMove.anchoredPosition = originalPosition;
    }
}