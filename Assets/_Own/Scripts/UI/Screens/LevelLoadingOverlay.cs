using UnityEngine;

/// Covers the screen while a level is loading.
public class LevelLoadingOverlay : TransitionableScreen,
    IEventReceiver<OnLevelBeganSwitching>,
    IEventReceiver<OnLevelFinishedSwitching>
{
    [SerializeField] FadeZoom fadeZoom;
    
    public void On(OnLevelBeganSwitching  message) => TransitionIn();
    public void On(OnLevelFinishedSwitching message) => this.Delay(0f, TransitionOut);
    
    protected override void OnTransitionIn () => fadeZoom.FadeIn (canvasGroup, transform);
    protected override void OnTransitionOut() => fadeZoom.FadeOut(canvasGroup, transform, waitForFadeIn: true);
}