using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

public class InsertCoinController : MyBehaviour, IEventReceiver<OnPlayerDeath>
{
    [SerializeField] PlayerController controller;
    [SerializeField] float delayTime = 2.0f;
    [SerializeField] float screenDuration = 10.0f;

    private bool canListenToControls = false;
    private float initialTimeScale = 1.0f;

    public void Start()
    {
        // TODO Having to have a direct reference to PlayerController sucks a lot.
        // Sort of defeats the purpose of sending it a broadcast message.
        if (!controller) controller = FindObjectOfType<PlayerController>();
        Assert.IsNotNull(controller);
    }

    public void Update()
    {
        if (!canListenToControls) return;

        if (Input.GetButtonDown("InsertCoin"))
        {
            canListenToControls = false;
            StopAllCoroutines();

            new OnPlayerWillRespawn(controller.previousPlatform)
                .SetDeliveryType(MessageDeliveryType.Immediate)
                .PostEvent();
        }
    }

    public void On(OnPlayerDeath message)
    {
        Delay(delayTime, StartInsertCoinCountdown);
    }

    private void StartInsertCoinCountdown()
    {
        TimeHelper.timeScale = 0.0f;
        canListenToControls = true;
        StartCoroutine(OnTimeExpire(screenDuration));
    }

    private void Delay(float delay, Action action)
    {
        StartCoroutine(DelayCoroutine(delay, action));
    }

    private IEnumerator OnTimeExpire(float duration)
    {
        yield return new WaitForSecondsRealtime(duration);

        TimeHelper.timeScale = initialTimeScale;
        new OnGameOver().SetDeliveryType(MessageDeliveryType.Immediate).PostEvent();
    }

    private IEnumerator DelayCoroutine(float delay, Action action)
    {
        yield return new WaitForSecondsRealtime(delay);
        action?.Invoke();
    }
}