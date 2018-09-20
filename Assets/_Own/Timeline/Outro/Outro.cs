using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Playables;

public class Outro : MonoBehaviour
{
    [SerializeField] PlayableDirector outroDirector;

    private bool wasTriggered;

    void Start()
    {
        if (!outroDirector) outroDirector = GetComponent<PlayableDirector>();
        Assert.IsNotNull(outroDirector);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (wasTriggered) return;
        if (!enabled) return;
        if (!other.CompareTag("Player")) return;

        outroDirector.Play();
        this.Delay((float)outroDirector.duration - 0.1f, EndGame);

        wasTriggered = true;
    }

    private void EndGame()
    {
        new OnGameOver(isVictory: true).SetDeliveryType(MessageDeliveryType.Immediate).PostEvent();
    }
}
