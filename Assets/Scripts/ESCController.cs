using UnityEngine;

public class ESCController : MonoBehaviour
{
    [SerializeField] private PoPupAnimation pupAnimation;

    private Transform _transform;
    private bool _isMenuShow = true;

    private void Awake()
    {
        _transform = transform;
    }
   
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape)) PressESC();
    }

    private void PressESC()
    {
        if (_isMenuShow) HideMenu();
        else ShowMenu();
    }

    private void HideMenu()
    {
        _isMenuShow=false;
        pupAnimation.HidePoPap(_transform);
    }

    public void ShowMenu()
    {
        _isMenuShow = true;
        pupAnimation.ShowPoPap(_transform);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
