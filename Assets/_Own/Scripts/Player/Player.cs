using UnityEngine;
using System.Collections;

public enum Multiplier
{
    Speed = 1,
}

[RequireComponent(typeof(PlayerController))]
public class Player : MyBehaviour, IEventReceiver<PowerUpCollected>
{
    private PlayerController playerController;
    private bool isGodMode = false;
    private float speedMultiplier;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    public void On(PowerUpCollected powerUpCollected)
    {
        if (isGodMode)
            return;

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
                playerController.SetFall(false);
                break;

            default: break;
        }

        StartCoroutine(OnPowerUpExpire(powerUp));
    }

    public bool IsGodMode()
    {
        return isGodMode;
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
            playerController.SetFall(true);
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
