using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PauseScreen : TransitionableScreen
{
    [SerializeField] float transitionDuration = 1f;
    [SerializeField] float maxScale = 2f;

    private float initialTimeScale = 1f;

    void Update()
    {
        if (!Input.GetButtonDown("Pause")) return;

        if (isCurrentlySelected) 
            TransitionOut();
        else 
            TransitionIn();
    }

    protected override void OnTransitionIn()
    {
        initialTimeScale = Time.timeScale;
        Time.timeScale = 0f;
        FadeInZoom();
        SelectFirstButton();
    }

    protected override void OnTransitionOut()
    {
        Time.timeScale = initialTimeScale;
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