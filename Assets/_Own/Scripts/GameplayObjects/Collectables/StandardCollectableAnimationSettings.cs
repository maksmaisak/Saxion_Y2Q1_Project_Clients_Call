using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class StandardCollectableAnimationSettings
{
    [Header("Disappear animation parameters")]
    [SerializeField] [Range(0.01f, 1f)] float animationDuration = 0.25f;
    [SerializeField] [Range(0f   , 5f)] float jumpHeight = 3f;
    [SerializeField] [Range(0f   , 5f)] float jumpPower  = 1f;
    [SerializeField] AudioSource disappearSoundSource;
    
    public Sequence PlayDisappearAnimation(GameObject gameObject)
    {
        Transform transform = gameObject.transform;
        transform.DOKill();

        var sequence = DOTween
            .Sequence()
            .Join(transform.DOScale(Vector3.zero, animationDuration).SetEase(Ease.InBack))
            .Join(transform.DOLocalJump(Vector3.up * jumpHeight, jumpPower, 1, animationDuration).SetRelative());
        
        if (disappearSoundSource && disappearSoundSource.clip)
        {
            disappearSoundSource.Play();
            sequence.AppendInterval(disappearSoundSource.clip.length);
        }

        return sequence;
    }
}