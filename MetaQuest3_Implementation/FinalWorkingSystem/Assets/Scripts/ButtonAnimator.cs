using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; // needed for pointer events
using TMPro; 

public class ButtonAnimator : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [Header("Scale Settings")]
    public float hoverScale = 1.1f;  // how much bigger on hover
    public float scaleSpeed = 10f;   // speed of scaling

    [Header("Color Settings")]
    public Color normalColor = Color.white;    // default color
    public Color pressedColor = Color.gray;    // color when pressed

    private Vector3 originalScale;
    private Image buttonImage;

    void Start()
    {
        originalScale = transform.localScale;
        buttonImage = GetComponent<Image>();

        if (buttonImage != null)
            buttonImage.color = normalColor;
    }

    void Update()
    {
        // Smoothly scale back to target scale
        Vector3 targetScale = originalScale;
        if (isHovered)
            targetScale = originalScale * hoverScale;

        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * scaleSpeed);
    }

    private bool isHovered = false;

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (buttonImage != null)
            buttonImage.color = pressedColor;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (buttonImage != null)
            buttonImage.color = normalColor;
    }
}