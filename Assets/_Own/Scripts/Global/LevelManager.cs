using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : Singleton<LevelManager>
{
    private readonly Stack<int> previousSceneBuildIndices = new Stack<int>();

    private bool isSceneBeingLoaded;
    private bool isPreloadedScreenAwaiting;
    
    public void LoadLevel(string levelSceneName)
    {        
        Debug.Log("LoadLevel called");
        new OnLevelBeginSwitching().SetDeliveryType(MessageDeliveryType.Immediate).PostEvent();

        Time.timeScale = 0f;
        SceneManager.LoadSceneAsync(levelSceneName).completed += ao =>
        {
            Time.timeScale = 1f;
            new OnLevelFinishSwitching().SetDeliveryType(MessageDeliveryType.Immediate).PostEvent();
        };
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