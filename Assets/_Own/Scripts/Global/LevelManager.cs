using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

/// TODO Make the level sequences editable in the inspector of this.
public class LevelManager : PersistentSingleton<LevelManager>, 
    IEventReceiver<OnPortalEntered>, IEventReceiver<OnGameModeSelect>
{
    [Header("Scene")]
    [SerializeField] string mainMenu = "main_menu";
    [Header("Story mode")]
    [SerializeField] string firstLevel = "Tutorial";
    [Header("Endless mode")]
    [SerializeField] List<string> endlessModeSceneNames = new List<string>();
    [Header("Bonus Level")]
    [SerializeField] List<string> bonusLevelSceneNames = new List<string>();

    private readonly Queue<string> bonusSceneNamesSequence = new Queue<string>();
    private readonly Queue<string> endlessSceneNamesSequence = new Queue<string>();
    private readonly Stack<int> previousSceneBuildIndices = new Stack<int>();

    private bool isSceneBeingLoaded;
    private bool isPreloadedScreenAwaiting;

    private void Start()
    {
        RandomizeSceneSequences();
    }

    public void On(OnGameModeSelect mode)
    {
        if (mode.selectedGameMode == GameMode.Story)
            StartStoryMode();
        else
            StartEndlessMode();
    }

    private void StartStoryMode()
    {
        LoadLevel(firstLevel);

        new OnGameplaySessionStarted()
            .SetDeliveryType(MessageDeliveryType.Immediate)
            .PostEvent();
    }
    
    private void StartEndlessMode()
    {
        LoadNextEndlessModeScene();

        new OnGameplaySessionStarted()
            .SetDeliveryType(MessageDeliveryType.Immediate)
            .PostEvent();
    }

    private void LoadLevel(string levelSceneName, bool pauseTimeWhileLoading = true)
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

    private void LoadNextEndlessModeScene()
    {
        Assert.IsTrue(endlessSceneNamesSequence.Count > 0);

        string sceneName = endlessSceneNamesSequence.Dequeue();
        LoadLevel(sceneName, pauseTimeWhileLoading: false);
        endlessSceneNamesSequence.Enqueue(sceneName);
    }

    private void LoadNextBonusLevel()
    {
        Assert.IsTrue(bonusSceneNamesSequence.Count > 0);

        string sceneName = bonusSceneNamesSequence.Dequeue();
        LoadLevel(sceneName, pauseTimeWhileLoading: false);
        bonusSceneNamesSequence.Enqueue(sceneName);
    }

    private void RandomizeSceneSequences()
    {
        /// Random shuffle lists using the Fisher-Yates algorithm
        for (int i = bonusLevelSceneNames.Count - 1; i >= 1; i--)
        {
            int j = Random.Range(0, i);
            bonusLevelSceneNames[j] = bonusLevelSceneNames[i];
        }

        foreach (string sceneName in bonusLevelSceneNames)
            bonusSceneNamesSequence.Enqueue(sceneName);

        for (int i = endlessModeSceneNames.Count - 1; i >= 1; i--)
        {
            int j = Random.Range(0, i);
            endlessModeSceneNames[j] = endlessModeSceneNames[i];
        }

        foreach (string sceneName in endlessModeSceneNames)
            endlessSceneNamesSequence.Enqueue(sceneName);
    }

    public void On(OnPortalEntered portal)
    {
        if (portal.kind == Portal.Kind.BonusLevelEntry && GlobalState.instance.currentGameMode == GameMode.Story)
            LoadLevel(portal.storyModeNextLevelScene, pauseTimeWhileLoading: false);
        else if(portal.kind == Portal.Kind.BonusLevelEntry && GlobalState.instance.currentGameMode == GameMode.Endless)
            LoadNextBonusLevel();
        else if (GlobalState.instance.currentGameMode == GameMode.Story)
            LoadLevel(portal.storyModeNextLevelScene, pauseTimeWhileLoading: false);
        else if (GlobalState.instance.currentGameMode == GameMode.Endless)
            LoadNextEndlessModeScene();
    }

    public void RestartGame()
    {
        if (GlobalState.instance.currentGameMode == GameMode.Story)
            StartStoryMode();
        else
            StartEndlessMode();
    }

    public void ReloadCurrentLevel()
    {
        LoadLevel(SceneManager.GetActiveScene().name);
    }
}
