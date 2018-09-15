using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : PersistentSingleton<LevelManager>
{
    private readonly Stack<int> previousSceneBuildIndices = new Stack<int>();

    private bool isSceneBeingLoaded;
    private bool isPreloadedScreenAwaiting;
    
    public void LoadLevel(string levelSceneName, bool pauseTimeWhileLoading = true)
    {
        new OnLevelBeginSwitching().SetDeliveryType(MessageDeliveryType.Immediate).PostEvent();

        if (pauseTimeWhileLoading) Time.timeScale = 0f;
        AsyncOperation load = SceneManager.LoadSceneAsync(levelSceneName);
        load.completed += ao =>
        {
            Time.timeScale = 1f;
            new OnLevelFinishSwitching().SetDeliveryType(MessageDeliveryType.Immediate).PostEvent();
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