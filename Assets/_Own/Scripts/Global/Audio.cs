using DG.Tweening;
using UnityEngine;
using UnityEngine.Assertions;

public class Audio : Singleton<Audio>, IEventReceiver<OnPowerUpCollected>
{
    [SerializeField] AudioSource soundtrackAudioSource;
    [SerializeField] AudioClip soundtrack;
    [SerializeField] float pitchChangeDuration = 0.5f;
    [SerializeField] float volumeChangeDuration = 0.1f;
    [SerializeField] float pitchMultiplierWhenSlowPowerup = 0.5f;
    [SerializeField] float pitchMultiplierWhenFastPowerup = 2f;

    private float soundtrackDefaultPitch;
    private float soundtrackDefaultVolume;

    private Tween soundtrackPitchTween;
    private Tween soundtrackVolumeTween;

    void Start()
    {
        if (!soundtrack) return;
        if (!soundtrackAudioSource) return;

        soundtrackDefaultPitch = soundtrackAudioSource.pitch;
        soundtrackDefaultVolume = soundtrackAudioSource.volume;

        soundtrackAudioSource.clip = soundtrack;
        soundtrackAudioSource.loop = true;
        soundtrackAudioSource.Play();
    }

    public void On(OnPowerUpCollected message)
    {
        float targetPitch = soundtrackDefaultPitch * GetTargetPitch(message.powerUpInfo.type);

        soundtrackPitchTween?.Kill();
        soundtrackPitchTween = DOTween
            .Sequence()
            .SetUpdate(isIndependentUpdate: true)
            .AppendInterval(message.powerUpInfo.duration)
            .Join(TweenSoundtrackPitch(targetPitch))
            .Append(TweenSoundtrackPitch(soundtrackDefaultPitch));

    }

    public void SetMusicVolume(float normalizedTargetVolume)
    {
        soundtrackVolumeTween?.Kill();
        soundtrackVolumeTween = soundtrackAudioSource
            .DOFade(normalizedTargetVolume * soundtrackDefaultVolume, volumeChangeDuration);
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