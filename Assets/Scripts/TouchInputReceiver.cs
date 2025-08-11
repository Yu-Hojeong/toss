
using UnityEngine;
using UnityEngine.EventSystems; 

public class TouchInputReceiver : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public PlayerController playerController;
    

   
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("TouchInputReceiver: OnPointerDownEvent via Event Trigger called!");
        playerController.OnScreenTouchDown(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        playerController.OnScreenDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        playerController.OnScreenTouchUp(eventData);
    }
}