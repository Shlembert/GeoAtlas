using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PinDataHandler : MonoBehaviour
{
    [SerializeField] private TMP_InputField titleInputField;
    [SerializeField] private TMP_InputField descriptionInputField;
    [SerializeField] private TMP_InputField detailedDescriptionInputField;
    [SerializeField] private TouchController touchController;
    [SerializeField] private Image pinImage;

    private string _title;
    private string _description;
    private string _detailedDescription;
    private Sprite _pinImage;
    private Vector3 _pinCoordinates;

    public void LoadData(string title, string description, string detailedDescription, Sprite image, Vector3 coordinates)
    {
        _title = title;
        _description = description;
        _detailedDescription = detailedDescription;
        _pinImage = image;
        _pinCoordinates = coordinates;

        UpdateUI();
    }

    private void UpdateUI()
    {
        titleInputField.text = _title;
        descriptionInputField.text = _description;
        detailedDescriptionInputField.text = _detailedDescription;
        pinImage.sprite = _pinImage;
    }

    public (string title, string description, string detailedDescription, Sprite image, Vector3 coordinates) ReadData()
    {
        return (_title, _description, _detailedDescription, _pinImage, _pinCoordinates);
    }

    public void ClearFields()
    {
        _title = string.Empty;
        _description = string.Empty;
        _detailedDescription = string.Empty;
        _pinImage = null;
        _pinCoordinates = Vector3.zero;

        UpdateUI();
    }

    public void AssignDataFromUI()
    {
        _title = titleInputField.text;
        _description = descriptionInputField.text;
        _detailedDescription = detailedDescriptionInputField.text;
        _pinCoordinates = touchController.ClickPosition;
        _pinImage = pinImage.sprite;
    }
}
