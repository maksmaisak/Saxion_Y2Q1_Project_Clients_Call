using DG.Tweening;
using UnityEngine;
using UnityEngine.Assertions;

public class Audio : Singleton<Audio>, IEventReceiver<OnPowerUpCollected>
{
    [SerializeField] AudioSource soundtrackAudioSource;
    [SerializeField] AudioClip soundtrack;
    [SerializeField] float pitchChangeDuration = 0.5f;
    [SerializeField] float pitchMultiplierWhenSlowPowerup = 0.5f;
    [SerializeField] float pitchMultiplierWhenFastPowerup = 2f;

    private float soundtrackDefaultPitch;

    void Start()
    {
        if (!soundtrack) return;
        if (!soundtrackAudioSource) return;

        soundtrackDefaultPitch = soundtrackAudioSource.pitch;

        soundtrackAudioSource.clip = soundtrack;
        soundtrackAudioSource.loop = true;
        soundtrackAudioSource.Play();
    }

    public void On(OnPowerUpCollected message)
    {
        float targetPitch = soundtrackDefaultPitch * GetTargetPitch(message.powerUpInfo.type);

        soundtrackAudioSource.DOKill(complete: false);
        DOTween
            .Sequence()
            .SetUpdate(isIndependentUpdate: true)
            .AppendInterval(message.powerUpInfo.duration)
            .Join(TweenSoundtrackPitch(targetPitch))
            .Append(TweenSoundtrackPitch(soundtrackDefaultPitch));

    }

    private Tween TweenSoundtrackPitch(float targetPitch)
    {
        return soundtrackAudioSource
            .DOPitch(targetPitch, pitchChangeDuration)
            .SetUpdate(isIndependentUpdate: true);
    }

    private float GetTargetPitch(PowerUpType powerUpType)
    {
        switch (powerUpType)
        {
            case PowerUpType.Slow: return pitchMultiplierWhenSlowPowerup;
            case PowerUpType.Fast: return pitchMultiplierWhenFastPowerup;
        }

        return soundtrackDefaultPitch;
    }
}