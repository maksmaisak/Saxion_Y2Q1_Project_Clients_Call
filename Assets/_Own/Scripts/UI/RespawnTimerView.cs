using TMPro;
using System;
using UnityEngine;
using UnityEngine.Assertions;

public class RespawnTimerView : MyBehaviour, IEventReceiver<OnPlayerWillRespawn>
{
    [SerializeField] float maxTimerDuration = 5.0f;
    [SerializeField] TMP_Text timerText;
    [SerializeField] TMP_Text textView;

    private bool canShowTimer = false;
    private float timeLeft;

    private void Start()
    {
        Assert.IsNotNull(timerText);
        Assert.IsNotNull(textView);
    }

    private void Update()
    {
        if (!canShowTimer) return;

        timeLeft -= Time.unscaledDeltaTime;
        timerText.SetText(Math.Round(timeLeft, 0).ToString());

        if (timeLeft <= 0)
            ShowTimer(false);
    }

    public void On(OnPlayerWillRespawn message)
    {
        ShowTimer(true);
    }

    private void ShowTimer(bool enable)
    {
        canShowTimer = enable;
        timeLeft = maxTimerDuration;
        timerText.enabled = enable;
        textView.enabled = enable;
    }
}

