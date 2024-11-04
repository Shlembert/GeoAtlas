using System.Collections.Generic;
using UnityEngine;

public class PinService : MonoBehaviour
{
    [SerializeField] private GameObject pinPrefab;
    [SerializeField] private PinDataHandler dataHandler;
    [SerializeField] private TouchController touchController;
    [SerializeField] private PinSaveLoadService pinSaveLoadService;
    [SerializeField] private ImageLoader imageLoader;
    [SerializeField] private List<PinData> _pins = new List<PinData>();

    private Transform _transform;
    private PinController _currentPinController;
    private bool _isEdit = false;

    public bool IsEdit { get => _isEdit; set => _isEdit = value; }
    public PinController CurrentPinController { get => _currentPinController; set => _currentPinController = value; }

    private void Awake()
    {
        _transform = transform;
        LoadPins();
    }

    public async void CreatePin()
    {
        if (_isEdit && _currentPinController)
        {
            _pins.Remove(_currentPinController.PinData);
            SetPinContent(_currentPinController.PinData);
            _pins.Add(_currentPinController.PinData);
            _isEdit = false;
            _currentPinController = null;
        }
        else
        {
            Vector3 position = touchController.ClickPosition;
            GameObject pin = Instantiate(pinPrefab, _transform.position, Quaternion.identity);
            pin.transform.SetParent(_transform, false);
            pin.transform.position = position;

            PinController pinController = pin.GetComponent<PinController>();
            pinController.PinData = new PinData();

            await imageLoader.SaveImageAsync();
            pinController.PinData.spriteAddress = imageLoader.GetSavedImagePath();

            SetPinContent(pinController.PinData);

            pin.GetComponent<PinAnimation>().ShowPin();

            _pins.Add(pinController.PinData);
        }

        SavePins();
    }


    public void SavePins()
    {
        if (_pins.Count > 0)
        {
            pinSaveLoadService.SavePinsToJson(_pins);
        }
        else
        {
            Debug.Log("Список пинов пуст!");
        }
    }

    private void SetPinContent(PinData pinData)
    {
        var (title, description, detailedDescription, image, coordinates) = dataHandler.ReadData();
        pinData.pinName = title;
        pinData.shortDescription = description;
        pinData.detailedDescription = detailedDescription;
        pinData.coordinates = coordinates;
    }

    public void DeletePin(GameObject pin)
    {
        PinController pinController = pin.GetComponent<PinController>();
        _pins.Remove(pinController.PinData);
        imageLoader.DeleteSavedImage(pinController.PinData.spriteAddress);
        Destroy(pin);
        SavePins();
    }

    public void LoadPins()
    {
        _pins.Clear();

        List<PinData> loadedPins = pinSaveLoadService.LoadPinsFromJson();
        foreach (PinData pinData in loadedPins)
        {
            GameObject pin = Instantiate(pinPrefab, _transform);
            pin.transform.position = pinData.coordinates;

            PinController pinController = pin.GetComponent<PinController>();
            pinController.PinData = pinData;
            Debug.Log($"Загружен пин: {pinData.pinName}");
            pin.GetComponent<PinAnimation>().ShowPin();
            _pins.Add(pinData);
        }
    }
}
