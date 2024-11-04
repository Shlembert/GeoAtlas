using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

[RequireComponent(typeof(ButtonAnimation))]
public class InputButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private UnityEvent onClick;

    private ButtonAnimation _animationButton;

    private void Awake()
    {
        _animationButton = GetComponent<ButtonAnimation>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _animationButton?.Increase();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _animationButton?.Relax();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _animationButton?.Decrease();
        onClick.Invoke(); 
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _animationButton?.Relax();
    }
}
