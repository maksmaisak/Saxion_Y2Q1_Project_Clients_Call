using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResolutionPlayAgain : MonoBehaviour
{
    public void OnPlayAgainSelect()
    {
        LevelManager.instance.RestartGame();
    }
}
