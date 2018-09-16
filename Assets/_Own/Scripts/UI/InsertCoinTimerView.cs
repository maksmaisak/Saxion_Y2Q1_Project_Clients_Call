using TMPro;
using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Assertions;

public class InsertCoinTimerView : MyBehaviour, IEventReceiver<OnPlayerDeath>,
    IEventReceiver<OnPlayerRespawn>
{
    [SerializeField] TMP_Text timerText;
    [SerializeField] TMP_Text textView;
    [SerializeField] Image backgroundImage;
    [SerializeField] float delayTime = 2.0f;
    [SerializeField] float maxTimerDuration = 10.0f;

    private bool canShowTimer = false;
    private float timeLeft;

    private void Start()
    {
        Assert.IsNotNull(timerText);
        Assert.IsNotNull(textView);
        Assert.IsNotNull(backgroundImage);
    }

    private void Update()
    {
        if (!canShowTimer) return;

        timeLeft -= Time.unscaledDeltaTime;
        timerText.SetText(Math.Round(timeLeft, 0).ToString());

        if (timeLeft <= 0)
            ShowTimer(false);
    }

    private void ShowTimer(bool enable)
    {
        canShowTimer = enable;
        timeLeft = maxTimerDuration;
        timerText.enabled = enable;
        textView.enabled = enable;
        backgroundImage.enabled = enable;
    }

    public void On(OnPlayerDeath message)
    {
        Delay(delayTime, ShowTimer);
    }

    public void On(OnPlayerRespawn message)
    {
        ShowTimer(false);
    }

    private void Delay(float delay, Action<bool> action)
    {
        StartCoroutine(DelayCoroutine(delay, action));
    }

    private IEnumerator DelayCoroutine(float delay, Action<bool> action)
    {
        yield return new WaitForSecondsRealtime(delay);
        action?.Invoke(true);
    }
}
