using TMPro;
using UnityEngine;

public class PowerUpTimerView : MyBehaviour, IEventReceiver<PowerUpCollected>
{
    [SerializeField] TMP_Text timerText;
    
    private bool canShowTimer = false;
    private float timeLeft;

    void Update()
    {
        if (!canShowTimer) return;
        
        timeLeft -= Time.unscaledDeltaTime;
        timerText.text = "Timer: " + System.Math.Round(timeLeft, 2);

        if (timeLeft <= 0)
        {
            canShowTimer = false;
            timeLeft = 0.0f;
            timerText.enabled = false;
        }
    }
    
    public void On(PowerUpCollected powerUpCollected)
    {
        StartTimer(powerUpCollected.powerUpInfo.duration);
    }

    private void StartTimer(float duration)
    {
        timeLeft = duration;
        canShowTimer = true;
        timerText.enabled = true;
    }
}
