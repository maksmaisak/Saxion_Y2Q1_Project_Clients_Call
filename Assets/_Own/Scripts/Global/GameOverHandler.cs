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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }

    public void GoToResolutionScreen()
    {
        var world = WorldRepresentation.instance;
        Assert.IsNotNull(world);
        
        SceneManager.LoadScene(SceneNames.mainResolutionScreenName, LoadSceneMode.Single);
        
        new OnResolutionScreen(world.playerScore, world.playerProfiles).PostEvent();
    }
}
