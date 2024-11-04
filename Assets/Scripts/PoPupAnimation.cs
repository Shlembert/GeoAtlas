using DG.Tweening;
using UnityEngine;

public class PoPupAnimation : MonoBehaviour
{
    [SerializeField] private float duration;

    public void ShowPoPap(Transform transform)
    {
        if (transform == null) return;
        transform.DOScale(Vector3.one, duration).SetEase(Ease.OutBack);
    }

    public void HidePoPap(Transform transform)
    {
        if (transform == null) return;
        transform.DOScale(Vector3.zero, duration).SetEase(Ease.InBack);
    }
}
