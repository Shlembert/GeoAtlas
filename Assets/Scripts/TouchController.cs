using UnityEngine;
using UnityEngine.EventSystems;

public class TouchController : MonoBehaviour, IPointerDownHandler
{
    [SerializeField]private PoPupAnimation pupAnimation;
    [SerializeField] private Transform pinCreate;

    private Vector3 _clickPosition;

    public Vector3 ClickPosition { get => _clickPosition; set => _clickPosition = value; }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.pointerEnter != null && eventData.pointerEnter.CompareTag("Map"))
        {
            Vector3 screenClickPosition = eventData.position;

            Vector3 worldClickPosition = Camera.main.
                ScreenToWorldPoint(new Vector3(screenClickPosition.x, screenClickPosition.y, screenClickPosition.z));

            _clickPosition = worldClickPosition;
            _clickPosition.z = 0;
            pupAnimation.ShowPoPap(pinCreate);
        }
    }
}
