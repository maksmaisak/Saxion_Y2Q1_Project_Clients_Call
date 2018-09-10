using UnityEngine;
using System.Collections;

public enum Multiplier
{
    Speed = 1,
}

[RequireComponent(typeof(PlayerController))]
public class Player : MyBehaviour, IEventReceiver<OnPowerUpCollected>
{
    public bool isGodMode { get; private set; }
    private float speedMultiplier;

    public void On(OnPowerUpCollected powerUpCollected)
    {
        if (isGodMode) return;

        PowerUpInfo powerUp = powerUpCollected.powerUpInfo;

        switch(powerUp.type)
        {
            case PowerUpType.Slow:
                Time.timeScale = powerUp.basepoints;
                Time.fixedDeltaTime = 0.02f * powerUp.basepoints;
                break;

            case PowerUpType.Fast:
                isGodMode = true;
                ApplyMultiplier(Multiplier.Speed, powerUp.basepoints);
                break;

            default: break;
        }

        StartCoroutine(OnPowerUpExpire(powerUp));
    }

    private IEnumerator OnPowerUpExpire(PowerUpInfo info)
    {
        yield return new WaitForSecondsRealtime(info.duration);

        if (info.type == PowerUpType.Slow)
        {
            Time.timeScale = 1.0f;
            Time.fixedDeltaTime = 0.02f;
        }
        else if(info.type == PowerUpType.Fast)
        {
            isGodMode = false;
            ApplyMultiplier(Multiplier.Speed, 0.0f);
        }
    }

    public void ApplyMultiplier(Multiplier multiplier, float amount)
    {
        switch (multiplier)
        {
            case Multiplier.Speed:
                speedMultiplier = amount;
                break;

            default: break;
        }
    }

    public float GetSpeedMultiplier()
    {
        return Mathf.Max(1.0f, speedMultiplier);
    }
}
