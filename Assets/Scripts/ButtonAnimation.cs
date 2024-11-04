using UnityEngine;
using DG.Tweening;

public class ButtonAnimation : MonoBehaviour
{
    [SerializeField] private float maxSize;
    [SerializeField] private float minSize;
    [SerializeField] private float duration;

    private Transform _transform;

    private void Awake() => _transform = transform;

    public void Increase() => _transform.DOScale(maxSize, duration).SetEase(Ease.OutBounce);

    public void Relax() => _transform.DOScale(Vector3.one, duration).SetEase(Ease.OutBack);

    public void Decrease() => _transform.DOScale(minSize, duration).SetEase(Ease.OutBack);
}
