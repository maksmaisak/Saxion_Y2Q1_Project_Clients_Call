using System;
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
            RestartScene
        };
    }

    public void On(OnGameOver message)
    {
        reactions?[(int)gameOverReaction]?.Invoke();
    }

    public void RestartScene()
    {
        LevelManager.instance.ReloadCurrentLevel();
    }

    public void GoToResolutionScreen()
    {
        var world = WorldRepresentation.instance;
        Assert.IsNotNull(world);

        var profiler = Profiler.instance;
        Assert.IsNotNull(profiler);
        
        var message = new OnResolutionScreen(world.playerScore, profiler.GetProfiles());
        profiler.Reset();
        
        SceneManager.LoadScene(SceneNames.mainResolutionScreenName, LoadSceneMode.Single);
        
        message.PostEvent();
    }
}
