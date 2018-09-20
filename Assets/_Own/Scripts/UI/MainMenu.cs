using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] TransitionableScreen gameModeSelectionScreen;

    void Start()
    {
        Debug.Assert(gameModeSelectionScreen);
        CursorHelper.Lock();
    }

    public void OnButtonStartSelect()
    {
        gameModeSelectionScreen.TransitionIn();
    }

    public void QuiToDesktop() => Quit.ToDesktop();
}