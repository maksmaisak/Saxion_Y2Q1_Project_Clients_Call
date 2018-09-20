using UnityEngine;

public class FadeinTransitionableScreen : TransitionableScreen
{
    [SerializeField] FadeZoom fadeZoom;

    protected override void OnTransitionIn()
    {
        fadeZoom.FadeIn(canvasGroup, transform);
        SelectFirstButton();
    }

    protected override void OnTransitionOut()
    {
        fadeZoom.FadeOut(canvasGroup, transform);
    }
}