using UnityEngine;
using UnityEngine.EventSystems;

public class VirtualJoystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public RectTransform joystickHandle;
    public float handleRange = 50f;
    private Vector2 inputVector;

    private void Start()
    {
        // Asigură-te că joystick-ul este inițial invizibil
        joystickHandle.gameObject.SetActive(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Setează poziția de start a joystick-ului
        joystickHandle.position = eventData.position;
        joystickHandle.gameObject.SetActive(true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Resetează joystick-ul
        inputVector = Vector2.zero;
        joystickHandle.anchoredPosition = Vector2.zero;
        joystickHandle.gameObject.SetActive(false);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(joystickHandle.parent as RectTransform, eventData.position, eventData.pressEventCamera, out position);
        position = Vector2.ClampMagnitude(position, handleRange);
        joystickHandle.anchoredPosition = position;
        inputVector = position / handleRange;
    }

    public Vector2 GetInputVector()
    {
        return inputVector;
    }
}
