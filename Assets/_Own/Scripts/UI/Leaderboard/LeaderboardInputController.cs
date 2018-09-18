using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Assertions;
using System.Collections.Generic;

public class LeaderboardInputController : MyBehaviour
{
    [SerializeField] char[] letters = new char[] {
        'A', 'B', 'C',
        'D', 'E', 'F',
        'G', 'H', 'I',
        'J', 'K', 'L',
        'M', 'N', 'O',
        'P', 'Q', 'R', 'S',
        'T', 'U', 'V',
        'W', 'X', 'Y', 'Z' };

    internal enum ButtonIndex
    {
        Left,
        Middle,
        Right,
        Check,
        Start,
        Quit,
        All,
    }

    [SerializeField] List<Button> buttons = new List<Button>();
    private Dictionary<ButtonIndex, int> buttonLetterIndexes = new Dictionary<ButtonIndex, int>();

    private Button currentButton;

    private ButtonIndex currentButtonIndex = ButtonIndex.Left;
    private bool isLetterUpdateRequired = false;

    private void Start()
    {
        buttonLetterIndexes.Add(ButtonIndex.Left, Random.Range(0, letters.Length - 1));
        buttonLetterIndexes.Add(ButtonIndex.Middle, Random.Range(0, letters.Length - 1));
        buttonLetterIndexes.Add(ButtonIndex.Right, Random.Range(0, letters.Length - 1));

        foreach (var button in GetComponentsInChildren<Button>())
            buttons.Add(button);

        for (int i = 0; i < (int)ButtonIndex.Check; i++)
        {
            Button button = buttons[i];
            button.GetComponentInChildren<TMP_Text>().text = letters[buttonLetterIndexes[(ButtonIndex)i]].ToString();
        }

        buttons[(int)ButtonIndex.Check].onClick.AddListener(() => OnSelectCheckBox());

        /// Bleah, delete this.
        Button playButton = GameObject.Find("Play").GetComponent<Button>();
        Button quitButton = GameObject.Find("Quit").GetComponent<Button>();

        buttons.Add(playButton);
        buttons.Add(quitButton);

        Navigation navigation = new Navigation();
        navigation.mode = Navigation.Mode.Explicit;
        navigation.selectOnDown = playButton;
        navigation.selectOnUp = playButton;
        navigation.selectOnRight = playButton;
        navigation.selectOnLeft = buttons[(int)ButtonIndex.Right];

        buttons[(int)ButtonIndex.Check].navigation = navigation;

        currentButton = buttons[(int)currentButtonIndex];
        currentButton.Select();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        buttons[(int)ButtonIndex.Check].onClick.RemoveListener(() => OnSelectCheckBox());
    }

    private void Update()
    {
        GameObject selectedGameobject = EventSystem.current.currentSelectedGameObject;

        if (selectedGameobject == null)
            return;

        currentButton = selectedGameobject.GetComponent<Button>();

        if (currentButton == null)
            return;

        currentButtonIndex = (ButtonIndex)buttons.IndexOf(currentButton);

        if (currentButtonIndex > ButtonIndex.Right || (int)currentButtonIndex == -1)
            return;

        CheckInputVertically();

        if (isLetterUpdateRequired)
        {
            isLetterUpdateRequired = false;

            currentButton.GetComponentInChildren<TMP_Text>().text =
                letters[buttonLetterIndexes[(ButtonIndex)currentButtonIndex]].ToString();
        }
    }

    public void OnSelectCheckBox()
    {
        for (int i = 0; i < (int)ButtonIndex.Start; i++)
            buttons[i].interactable = false;

        string newPlayerName =
            letters[buttonLetterIndexes[ButtonIndex.Left]].ToString() +
            letters[buttonLetterIndexes[ButtonIndex.Middle]].ToString() +
            letters[buttonLetterIndexes[ButtonIndex.Right]].ToString();

        new OnNameInput(newPlayerName)
            .SetDeliveryType(MessageDeliveryType.Immediate)
            .PostEvent();

        buttons[(int)ButtonIndex.Start].Select();

        Destroy(this.gameObject);
    }

    private void CheckInputVertically()
    {
        float inputY = Input.GetAxis("Vertical");

        if (!Input.GetButtonDown("Vertical"))
            return;

        if (inputY < -0.01f)
        {
            buttonLetterIndexes[(ButtonIndex)currentButtonIndex]++;
            if (buttonLetterIndexes[(ButtonIndex)currentButtonIndex] >= letters.Length)
                buttonLetterIndexes[(ButtonIndex)currentButtonIndex] = 0;
        }
        else if (inputY > 0.01f)
        {
            buttonLetterIndexes[(ButtonIndex)currentButtonIndex]--;
            if (buttonLetterIndexes[(ButtonIndex)currentButtonIndex] < 0)
                buttonLetterIndexes[(ButtonIndex)currentButtonIndex] = letters.Length - 1;
        }

        isLetterUpdateRequired = true;
    }
}

