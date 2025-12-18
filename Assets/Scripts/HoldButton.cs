using UnityEngine;
using UnityEngine.EventSystems; // Required for UI touch detection

public class HoldButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [HideInInspector]
    public bool isPressed = false;

    // Called when finger touches button
    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
    }

    // Called when finger lifts off button
    public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;
    }
}