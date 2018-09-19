using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// TODO Make the level sequences editable in the inspector of this.
public class LevelManager : PersistentSingleton<LevelManager>
{
    [Header("Scene")]
    [SerializeField] string mainMenu = "main_menu";
    [Header("Story mode")]
    [SerializeField] string firstLevel = "Tutorial";
    [Header("Endless mode (TODO)")]
    
    private readonly Stack<int> previousSceneBuildIndices = new Stack<int>();
        
    private bool isSceneBeingLoaded;
    private bool isPreloadedScreenAwaiting;

    public void StartStoryMode()
    {
        LoadLevel(firstLevel);
        new OnGameplaySessionStarted().SetDeliveryType(MessageDeliveryType.Immediate).PostEvent();
    }
    
    public void LoadLevel(string levelSceneName, bool pauseTimeWhileLoading = true)
    {
        new OnLevelBeganSwitching().SetDeliveryType(MessageDeliveryType.Immediate).PostEvent();

        if (pauseTimeWhileLoading) Time.timeScale = 0f;
        AsyncOperation load = SceneManager.LoadSceneAsync(levelSceneName);
        load.completed += ao =>
        {
            Time.timeScale = 1f;
            new OnLevelFinishedSwitching().SetDeliveryType(MessageDeliveryType.Immediate).PostEvent();
        };
    }

    public void ReloadCurrentLevel()
    {
        LoadLevel(SceneManager.GetActiveScene().name);
    }

    public void LoadPreviousLevel()
    {
        throw new NotImplementedException();
    }

    public void LoadRandomLevel()
    {
        throw new NotImplementedException();
    }
}