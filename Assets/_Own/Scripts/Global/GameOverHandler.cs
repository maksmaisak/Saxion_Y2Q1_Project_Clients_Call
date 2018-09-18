using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

public class GameOverHandler : MyBehaviour, IEventReceiver<OnGameOver>
{
    enum GameOverReaction
    {
        GoToResolutionScreen,
        RestartScene
    }
    private readonly Action[] reactions;

    [SerializeField] GameOverReaction gameOverReaction = GameOverReaction.GoToResolutionScreen;
    
    public GameOverHandler()
    {
        reactions = new Action[] 
        {
            GoToResolutionScreen,
            RestartCurrentLevel
        };
    }

    public void On(OnGameOver message)
    {
        reactions?[(int)gameOverReaction]?.Invoke();
    }

    public void RestartCurrentLevel()
    {
        LevelManager.instance.ReloadCurrentLevel();
    }

    public void GoToResolutionScreen()
    {
        var globalState = GlobalState.instance;        
        var message = new OnResolutionScreen(
            globalState.playerScore, 
            new Dictionary<PlayerProfile, int>(globalState.profiles)
        );
        
        SceneManager.LoadScene(SceneNames.mainResolutionScreenName, LoadSceneMode.Single);
        message.PostEvent();
    }
}
