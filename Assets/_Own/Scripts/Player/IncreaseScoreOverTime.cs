using System.Collections;
using TMPro;
using UnityEngine;

public class IncreaseScoreOverTime : MonoBehaviour
{
    [SerializeField] float updateScoreInterval = 0.5f;
    [SerializeField] int scoreIncreaseAmount = 3;

    private float intervalMultiplier = 1f; 

    void OnEnable()
    {
        StartCoroutine(IncreaseScoreCoroutine());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    IEnumerator IncreaseScoreCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(updateScoreInterval * intervalMultiplier);
            new OnScoreChange(scoreIncreaseAmount, playBumpEffect: false).PostEvent();
        }
    }

    void Update()
    {        
        intervalMultiplier = Input.GetKey(KeyCode.V) ? 0.1f : 1f;
    }
}