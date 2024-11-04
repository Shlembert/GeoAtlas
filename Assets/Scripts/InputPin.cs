using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputPin : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField] private PoPupAnimation poPupAnimation;
    [SerializeField] private Transform description;
    [SerializeField] private float holdDuration;
    [SerializeField] private PinController pinController;
    [SerializeField] private PinAnimation pinAnimation;
    [SerializeField] private PinService pinService;

    private bool isHolding = false;
    private float holdTime = 0f;
    private bool holdCompleted = false;

    private Vector3 dragOffset;

    private void SetContent()
    {
        DescriptionController desc = description.GetComponent<DescriptionController>();

        desc.NamePin.text = pinController.PinData.pinName;
        desc.Description.text = pinController.PinData.shortDescription;
        desc.DetailsDescription = pinController.PinData.detailedDescription;
        desc.CurrentPin = pinController;
        string imagePath = pinController.PinData.spriteAddress;

        if (File.Exists(imagePath))
        {
            byte[] fileData = File.ReadAllBytes(imagePath);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(fileData);

            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            desc.Image.sprite = sprite;
        }
        else
        {
            Debug.LogError("Изображение не найдено: " + imagePath);
        }
    }

    public void OnShortClick()
    {
        SetContent();
        poPupAnimation.ShowPoPap(description);
        Debug.Log("Короткое нажатие!");
    }

    public void OnHoldComplete()
    {
        pinAnimation.ActivePin();
        Debug.Log("Долгое нажатие завершено!");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isHolding = true;
        holdTime = 0f;
        holdCompleted = false;

        RectTransform rectTransform = GetComponent<RectTransform>();
        Vector3 mousePos;

        RectTransformUtility
            .ScreenPointToWorldPointInRectangle
            (rectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out mousePos);

        dragOffset = rectTransform.position - mousePos;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isHolding = false;
        pinAnimation.DeActivePin();

        if (!holdCompleted)
        {
            OnShortClick();
        }
        else
        {
            pinController.PinData.coordinates = transform.position;
            pinService.SavePins();
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (holdCompleted)
        {
            RectTransform rectTransform = GetComponent<RectTransform>();

            Vector3 worldPoint;
            RectTransformUtility
                .ScreenPointToWorldPointInRectangle
                (rectTransform,
                eventData.position,
                eventData.pressEventCamera,
                out worldPoint);

            rectTransform.position = worldPoint + dragOffset;
        }
    }

    private void Update()
    {
        if (isHolding)
        {
            holdTime += Time.deltaTime;

            if (holdTime >= holdDuration)
            {
                isHolding = false;
                holdCompleted = true;
                OnHoldComplete();
            }
        }
    }
}
