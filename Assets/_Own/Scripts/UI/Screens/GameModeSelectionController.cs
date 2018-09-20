using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public enum Difficulty
{
    Easy = 0,
    Medium,
    Hard,
    All,
}

public class GameModeSelectionController : MonoBehaviour
{
    internal enum ButtonIndex
    {
        Story,
        Endless,
        Difficulty,
        Back,
        All
    }

    [SerializeField] TMP_Text difficultyText;
    [SerializeField] List<string> difficultyNames = new List<string>();

    private List<Button> buttons = new List<Button>();
    private Difficulty currentDifficultyIndex = Difficulty.Medium;
    private GameObject currentSelectedObject = null;
    private ButtonIndex currentButtonIndex = ButtonIndex.Difficulty;

    private void Start()
    {
        foreach (var button in GetComponentsInChildren<Button>())
            buttons.Add(button);

        Assert.IsNotNull(difficultyText);
        Assert.IsTrue(buttons.Count == (int)ButtonIndex.All);

        buttons[(int)ButtonIndex.Difficulty].Select();

        difficultyText.SetText(difficultyNames[(int)currentDifficultyIndex]);
    }

    private void OnDisable()
    {
        buttons.Clear();
    }

    private void Update()
    {
        currentSelectedObject = EventSystem.current.currentSelectedGameObject;

        if (currentSelectedObject == null)
            return;

        Button button = currentSelectedObject.GetComponent<Button>();

        if (button == null)
            return;

        currentButtonIndex = (ButtonIndex)buttons.IndexOf(button);

        if (currentButtonIndex != ButtonIndex.Difficulty || (int)currentButtonIndex == -1)
            return;

        if (!CheckInputHorizontally())
            return;

        difficultyText.SetText(difficultyNames[(int)currentDifficultyIndex]);
    }

    private bool CheckInputHorizontally()
    {
        float inputX = Input.GetAxis("Horizontal");

        if (!Input.GetButtonDown("Horizontal"))
            return false;

        if (inputX > 0.01f) currentDifficultyIndex++;
        else if (inputX < -0.01f) currentDifficultyIndex--;

        if (currentDifficultyIndex < Difficulty.Easy)
            currentDifficultyIndex = Difficulty.Hard;
        else if (currentDifficultyIndex == Difficulty.All)
            currentDifficultyIndex = Difficulty.Easy;

        return true;
    }

    public void OnStoryModeSelect()
    {
        enabled = false;

        new OnGameModeSelect(GameMode.Story, currentDifficultyIndex)
            .SetDeliveryType(MessageDeliveryType.Immediate)
            .PostEvent();
    }

    public void OnEndlessModeSelect()
    {
        enabled = false;

        new OnGameModeSelect(GameMode.Endless, currentDifficultyIndex)
            .SetDeliveryType(MessageDeliveryType.Immediate)
            .PostEvent();
    }
}
