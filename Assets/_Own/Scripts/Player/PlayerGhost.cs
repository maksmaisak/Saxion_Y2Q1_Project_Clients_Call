using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public class PlayerGhost : MonoBehaviour
{
    [SerializeField] TMP_Text text;

    public TMP_Text playerGhostText { get { return text; } }

    private void Awake()
    {
        Assert.IsNotNull(text);
    }
}
