using UnityEngine;

public class GameModeSelectionScreen : TransitionableScreen
{
    [SerializeField] FadeZoom fadeZoom;

    protected override void Start()
    {
        base.Start();
    }

    protected override void OnTransitionIn()
    {
        fadeZoom.FadeIn(canvasGroup, transform);
        SelectFirstButton();
        GetComponent<GameModeSelectionController>().enabled = true;
    }

    protected override void OnTransitionOut()
    {
        fadeZoom.FadeOut(canvasGroup, transform);
    }

    public void OnBackButtonSelect()
    {
        previousScreens.Peek().TransitionIn();
        GetComponent<GameModeSelectionController>().enabled = false;
    }
}
