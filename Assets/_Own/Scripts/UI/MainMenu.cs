using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    void Start()
    {
        CursorHelper.Lock();
    }

    public void StartGame()
    {
        LevelManager.instance.StartStoryMode();
    }

    public void QuiToDesktop() => Quit.ToDesktop();
}