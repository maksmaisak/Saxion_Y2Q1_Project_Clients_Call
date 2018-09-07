using TMPro;
using UnityEngine;

public class PowerUpTimerView : MyBehaviour, IEventReceiver<PowerUpCollected>
{
    [SerializeField] private TMP_Text timerText;
    private bool canShowTimer = false;
    private float timerDuration;

    private void Update()
    {
        if (canShowTimer)
        {
            timerDuration -= Time.unscaledDeltaTime;
            timerText.text = "Timer: " + System.Math.Round(timerDuration, 2);

            if (timerDuration <= 0)
            {
                canShowTimer = false;
                timerDuration = 0.0f;
                timerText.enabled = false;
            }
        }
    }

    private void StartTimer(float duration)
    {
        timerDuration = duration;
        canShowTimer = true;
        timerText.enabled = true;
    }


    public void On(PowerUpCollected powerUpCollected)
    {
        StartTimer(powerUpCollected.powerUpInfo.duration);
    }
}
