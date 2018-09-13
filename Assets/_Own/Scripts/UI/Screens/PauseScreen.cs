using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PauseScreen : TransitionableScreen, 
    IEventReceiver<OnPause>, 
    IEventReceiver<OnUnpause>
{
    [SerializeField] float transitionDuration = 1f;
    [SerializeField] float maxScale = 2f;

    public void On(OnPause message)   => TransitionIn();
    public void On(OnUnpause message) => TransitionOut();
    
    protected override void OnTransitionIn()
    {
        FadeInZoom();
        SelectFirstButton();
    }

    protected override void OnTransitionOut()
    {
        FadeOutZoom();
    }

    private void FadeInZoom()
    {
        canvasGroup.DOKill(complete: true);
        canvasGroup
            .DOFade(1f, transitionDuration)
            .SetEase(Ease.InOutSine)
            .SetUpdate(isIndependentUpdate: true);

        transform.DOKill(complete: true);
        transform.localScale = Vector3.one;
        transform
            .DOScale(maxScale, transitionDuration)
            .From()
            .SetEase(Ease.OutExpo)
            .SetUpdate(isIndependentUpdate: true);
    }

    private void FadeOutZoom()
    {
        canvasGroup.DOKill(complete: true);
        canvasGroup
            .DOFade(0f, transitionDuration)
            .SetEase(Ease.InOutSine)
            .SetUpdate(isIndependentUpdate: true);

        transform.DOKill(complete: true);
        transform
            .DOScale(maxScale, transitionDuration)
            .SetEase(Ease.InExpo)
            .SetUpdate(isIndependentUpdate: true);
    }
}