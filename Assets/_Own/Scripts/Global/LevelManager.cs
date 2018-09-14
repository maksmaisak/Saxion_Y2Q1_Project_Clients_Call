using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : Singleton<LevelManager>
{
    private readonly Stack<int> previousSceneBuildIndices = new Stack<int>();

    private bool isSceneBeingLoaded;
    private bool isPreloadedSceenAwaiting;
    
    public void LoadLevel(string levelSceneName)
    {        
        Debug.Log("LoadLevel called");
        //new OnLevelBeginSwitching().PostEvent();
        
        SceneManager.LoadScene(levelSceneName);
 
        //new OnLevelSwitched().PostEvent();
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