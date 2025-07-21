using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class TMPUnderlineHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    public TMP_Text tmpText;
    private string originalText;
    private bool isHovered = false;
    private bool isSelected = false;

    void Start()
    {
        if (tmpText != null)
            originalText = tmpText.text;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
        UpdateUnderline();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
        UpdateUnderline();
    }

    public void OnSelect(BaseEventData eventData)
    {
        isSelected = true;
        UpdateUnderline();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        isSelected = false;
        UpdateUnderline();
    }

    void UpdateUnderline()
    {
        if (tmpText == null) return;

        if (isHovered || isSelected)
        {
            tmpText.text = $"<u>{originalText}</u>";
        }
        else
        {
            tmpText.text = originalText;
        }
    }
}
