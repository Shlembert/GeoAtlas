using UnityEngine;

public class PinController : MonoBehaviour
{
    [SerializeField] private DescriptionController descriptionController;

    private PinData _pinData;

    public PinData PinData { get => _pinData; set => _pinData = value; }
}
