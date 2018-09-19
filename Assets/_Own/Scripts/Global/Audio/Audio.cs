using DG.Tweening;
using UnityEngine;
using UnityEngine.Assertions;

public class Audio : Singleton<Audio>, IEventReceiver<OnPowerUpCollected>
{
    [Header("Soundtrack")]
    [SerializeField] AudioSource soundtrackAudioSource;
    [SerializeField] AudioClip soundtrack;
    [SerializeField] float pitchChangeDuration = 0.5f;
    [SerializeField] float volumeChangeDuration = 0.1f;
    [SerializeField] float pitchMultiplierWhenSlowPowerup = 0.5f;
    [SerializeField] float pitchMultiplierWhenFastPowerup = 2f;
    [Header("Effects")]

    private float soundtrackDefaultPitch;
    private float soundtrackDefaultVolume;

    private Tween soundtrackPitchTween;
    private Tween soundtrackVolumeTween;

    protected override void Awake()
    {
        base.Awake();
        
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

    public void SetMusicVolume(float normalizedTargetVolume, bool immediate = false)
    {
        soundtrackVolumeTween?.Kill();
        float targetVolume = normalizedTargetVolume * soundtrackDefaultVolume;
        if (immediate)
        {
            soundtrackAudioSource.volume = targetVolume;
            return;
        }
        
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