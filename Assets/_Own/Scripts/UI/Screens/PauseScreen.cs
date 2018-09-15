using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PauseScreen : TransitionableScreen, 
    IEventReceiver<OnPause>, 
    IEventReceiver<OnUnpause>
{
    [SerializeField] FadeZoom fadeZoom;

    public void On(OnPause   message) => TransitionIn();
    public void On(OnUnpause message) => TransitionOut();
    
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