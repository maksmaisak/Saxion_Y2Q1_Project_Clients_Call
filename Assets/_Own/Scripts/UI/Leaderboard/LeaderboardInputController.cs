using TMPro;
using UnityEngine;
using UnityEngine.UI;
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

    internal enum LetterBox
    {
        Left,
        Middle,
        Right,
        All,
    }

    [SerializeField] List<Button> disabledButtons = new List<Button>();
    [SerializeField] List<Button> buttons = new List<Button>();

    private Dictionary<LetterBox, int> buttonLetterIndexes = new Dictionary<LetterBox, int>();

    private Button currentButton;

    private int currentLetterBoxIndex = 0;
    private bool isLetterUpdateRequired = false;

    private void Start()
    {
        buttonLetterIndexes.Add(LetterBox.Left, Random.Range(0, letters.Length - 1));
        buttonLetterIndexes.Add(LetterBox.Middle, Random.Range(0, letters.Length - 1));
        buttonLetterIndexes.Add(LetterBox.Right, Random.Range(0, letters.Length - 1));

        foreach (var button in GetComponentsInChildren<Button>())
            buttons.Add(button);

        for (int i = 0; i < buttons.Count; i++)
        {
            Button button = buttons[i];
            button.GetComponentInChildren<TMP_Text>().text = letters[buttonLetterIndexes[(LetterBox)i]].ToString();
            button.onClick.AddListener(() => OnSelectBox());
        }

        /// Bleah, delete this.
        disabledButtons.Add(GameObject.Find("Play").GetComponent<Button>());
        disabledButtons.Add(GameObject.Find("Quit").GetComponent<Button>());

        foreach (var button in disabledButtons)
            button.interactable = false;

        currentButton = buttons[currentLetterBoxIndex];
        currentButton.Select();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        foreach (var button in buttons)
            button.onClick.RemoveListener(() => OnSelectBox());
    }

    private void Update()
    {
        CheckInputVertically();

        currentButton = buttons[currentLetterBoxIndex];

        if (currentButton == null)
            return;

        currentButton.Select();

        if (isLetterUpdateRequired)
        {
            isLetterUpdateRequired = false;

            currentButton.GetComponentInChildren<TMP_Text>().text =
                letters[buttonLetterIndexes[(LetterBox)currentLetterBoxIndex]].ToString();
        }
    }

    public void OnSelectBox()
    {
        currentButton.interactable = false;
        currentLetterBoxIndex++;

        if(currentLetterBoxIndex > 2)
        {
            string newPlayerName =
                letters[buttonLetterIndexes[LetterBox.Left]].ToString() +
                letters[buttonLetterIndexes[LetterBox.Middle]].ToString() +
                letters[buttonLetterIndexes[LetterBox.Right]].ToString();

            foreach (var button in disabledButtons)
                button.interactable = true;

            disabledButtons[0].Select();

            new OnNameInput(newPlayerName)
                .SetDeliveryType(MessageDeliveryType.Immediate)
                .PostEvent();

            Destroy(this.gameObject);
            return;
        }

        currentButton = buttons[currentLetterBoxIndex];
    }

    private void CheckInputVertically()
    {
        float inputY = Input.GetAxis("Vertical");

        if (!Input.GetButtonDown("Vertical"))
            return;

        if (inputY < -0.01f)
        {
            buttonLetterIndexes[(LetterBox)currentLetterBoxIndex]++;
            if (buttonLetterIndexes[(LetterBox)currentLetterBoxIndex] >= letters.Length)
                buttonLetterIndexes[(LetterBox)currentLetterBoxIndex] = 0;
        }
        else if (inputY > 0.01f)
        {
            buttonLetterIndexes[(LetterBox)currentLetterBoxIndex]--;
            if (buttonLetterIndexes[(LetterBox)currentLetterBoxIndex] < 0)
                buttonLetterIndexes[(LetterBox)currentLetterBoxIndex] = letters.Length - 1;
        }

        isLetterUpdateRequired = true;
    }
}

