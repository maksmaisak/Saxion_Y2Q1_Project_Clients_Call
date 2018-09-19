using UnityEngine;
using UnityEngine.Assertions;

public class SplashScreen : TransitionableScreen
{
    [SerializeField] FadeZoom fadeZoom;
    [SerializeField] TransitionableScreen menuScreen;

    protected override void Start()
    {
        base.Start();
        Assert.IsNotNull(menuScreen);
    }

    void Update()
    {
        if (isCurrentlySelected && Input.anyKeyDown)
        {
            menuScreen.TransitionIn();
        }
    }
    
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