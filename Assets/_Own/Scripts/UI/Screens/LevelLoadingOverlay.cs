using UnityEngine;

/// Covers the screen while a level is loading.
public class LevelLoadingOverlay : TransitionableScreen,
    IEventReceiver<OnLevelBeginSwitching>,
    IEventReceiver<OnLevelFinishSwitching>
{
    [SerializeField] FadeZoom fadeZoom;
    
    public void On(OnLevelBeginSwitching  message) => TransitionIn();
    public void On(OnLevelFinishSwitching message) => this.Delay(0f, TransitionOut);
    
    protected override void OnTransitionIn () => fadeZoom.FadeIn (canvasGroup, transform);
    protected override void OnTransitionOut() => fadeZoom.FadeOut(canvasGroup, transform);
}