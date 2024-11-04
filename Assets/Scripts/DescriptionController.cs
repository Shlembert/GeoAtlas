using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DescriptionController : MonoBehaviour
{
    [SerializeField] private ImageLoader imageLoader;
    [SerializeField] private DescriptionController detailsDesc;
    [SerializeField] private PinService pinService;
    [SerializeField] private PinDataHandler pinDataHandler;
    [SerializeField] private TMP_Text namePin, description;
    [SerializeField] private Image image;

    private PinController _currentPin;
    private string _detailsDescription;
    

    public TMP_Text NamePin { get => namePin; set => namePin = value; }
    public TMP_Text Description { get => description; set => description = value; }
    public Image Image { get => image; set => image = value; }
    public string DetailsDescription { get => _detailsDescription; set => _detailsDescription = value; }
    public PinController CurrentPin { get => _currentPin; set => _currentPin = value; }

    public void SetContentDetailsDescription()
    {
        detailsDesc.NamePin.text = namePin.text;
        detailsDesc.Description.text = _detailsDescription;
        detailsDesc.Image.sprite = image.sprite;
        detailsDesc.CurrentPin = _currentPin;
    }

    public void DeletePin()
    {
        pinService.DeletePin(_currentPin.gameObject);
    }

    public void EditPinData()
    {
        pinService.IsEdit = true;
        pinService.CurrentPinController = _currentPin;

        pinDataHandler.LoadData
            (_currentPin.PinData.pinName,
            _currentPin.PinData.shortDescription,
            _currentPin.PinData.detailedDescription,
            image.sprite, 
            _currentPin.PinData.coordinates);
    }
}
