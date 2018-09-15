using System;
using DG.Tweening;
using UnityEngine;

[Serializable]
public class FadeZoom
{
    [SerializeField] float transitionDuration = 0.1f;
    [SerializeField] float fadedOutScale = 2f;
    
    public void FadeIn(CanvasGroup canvasGroup, Transform transform)
    {
        canvasGroup.DOKill(complete: true);
        canvasGroup
            .DOFade(1f, transitionDuration)
            .SetEase(Ease.InOutSine)
            .SetUpdate(isIndependentUpdate: true);

        transform.DOKill(complete: true);
        transform.localScale = Vector3.one;
        transform
            .DOScale(fadedOutScale, transitionDuration)
            .From()
            .SetEase(Ease.OutExpo)
            .SetUpdate(isIndependentUpdate: true);
    }

    public void FadeOut(CanvasGroup canvasGroup, Transform transform)
    {
        canvasGroup.DOKill(complete: true);
        canvasGroup
            .DOFade(0f, transitionDuration)
            .SetEase(Ease.InOutSine)
            .SetUpdate(isIndependentUpdate: true);

        transform.DOKill(complete: true);
        transform
            .DOScale(fadedOutScale, transitionDuration)
            .SetEase(Ease.InExpo)
            .SetUpdate(isIndependentUpdate: true);
    }
}
