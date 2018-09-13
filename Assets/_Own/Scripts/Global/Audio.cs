using DG.Tweening;
using UnityEngine;
using UnityEngine.Assertions;

public class Audio : Singleton<Audio>, IEventReceiver<OnPowerUpCollected>
{
    [SerializeField] AudioSource soundtrackAudioSource;
    [SerializeField] AudioClip soundtrack;
    [SerializeField] float soundtrackPitchWhenSlowPowerup = 0.5f;
    [SerializeField] float soundtrackPitchWhenFastPowerup = 2f;

    private float soundtrackDefaultPitch;
    
    void Start()
    {
        if (!soundtrack) return;
        if (!soundtrackAudioSource) return;
        
        soundtrackAudioSource.clip = soundtrack;
        soundtrackAudioSource.loop = true;
        soundtrackAudioSource.Play();
    }

    public void On(OnPowerUpCollected message)
    {
        float targetPitch = GetTargetPitch(message.powerUpInfo.type);

        soundtrackAudioSource.DOKill(complete: false);
        soundtrackAudioSource.DOPitch(soundtrackPitchWhenSlowPowerup, targetPitch);
    }

    private float GetTargetPitch(PowerUpType powerUpType)
    {
        switch (powerUpType)
        {
            case PowerUpType.Slow: return soundtrackPitchWhenSlowPowerup;
            case PowerUpType.Fast: return soundtrackPitchWhenFastPowerup;
        }

        return soundtrackDefaultPitch;
    }
}