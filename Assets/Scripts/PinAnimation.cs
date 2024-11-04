using DG.Tweening;
using UnityEngine;

public class PinAnimation : MonoBehaviour
{
    [SerializeField] private Transform top, bottom;
    [SerializeField] private float duration, moveUpPos;

    private Sequence _sequence;
    private bool _isHide = true;

    public void ShowPin()
    {
        bottom.DOScale(Vector3.one, duration).SetEase(Ease.OutBack).OnComplete(() =>
        {
            top.DOScale(Vector3.one, duration).SetEase(Ease.OutBack);
            _isHide = false;
        });
    }

    public void HidePin()
    {
        DeActivePin();

        top.DOScale(Vector3.zero, duration).SetEase(Ease.InBack).OnComplete(() =>
        {
            bottom.DOScale(Vector3.zero, duration).SetEase(Ease.InBack);
            _isHide = true;
        });
    }

    public void ActivePin()
    {
        if (_isHide) return;
        float normalY = top.localPosition.y;

        _sequence = DOTween.Sequence();
        _sequence.Append(top.DOLocalMoveY(top.localPosition.y + moveUpPos, duration, false).SetEase(Ease.InOutSine))
                 .Append(top.DOLocalMoveY(normalY, duration, false).SetEase(Ease.InOutSine))
                 .SetLoops(-1, LoopType.Yoyo);
    }

    public void DeActivePin()
    {
        if (_isHide) return;

        _sequence.Kill();
        top.localPosition = Vector3.zero;
    }
}
