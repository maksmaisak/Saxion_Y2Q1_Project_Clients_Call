using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Assertions;

public class Cage : Collectable
{
    [Space] 
    [SerializeField] int scoreBonus = 100;

    [Header("Animation")]
    [SerializeField] Transform cage;
    [SerializeField] Transform bird;
    [SerializeField] float animationDuration = 0.25f;
    [Header("Cage jump")]
    [SerializeField] Vector3 cageJumpOffset = Vector3.back;
    [SerializeField] float cageJumpPower = 0.25f;
    [Header("Bird jump")]
    [SerializeField] Vector3 birdJumpOffset = Vector3.forward;
    [SerializeField] float birdJumpPower = 0.25f;
    [Header("Bird rise")]
    [SerializeField] float birdRiseHeight = 5f;
    [SerializeField] float birdRiseDuration = 0.375f;
    [SerializeField] Ease birdRiseEase = Ease.InQuart;
    [Header("Sound")]
    [SerializeField] AudioSource soundSource;

    protected override void Start()
    {
        base.Start();
        Assert.IsNotNull(cage);
        Assert.IsNotNull(bird);
    }
        
    protected override void OnCollected()
    {
        new OnCageOpen().PostEvent();
        new OnScoreChange(scoreBonus).PostEvent();
        
        PlayOpenAnimation();
    }

    private void PlayOpenAnimation()
    {
        var seq = DOTween.Sequence();
        seq.Join(cage.DOJump(cageJumpOffset, cageJumpPower, 1, animationDuration).SetRelative());
        seq.Join(Fade(cage));

        seq.Join(bird.DOJump(birdJumpOffset, birdJumpPower, 1, animationDuration).SetRelative());
        seq.Append(bird.DOMoveY(5f, birdRiseDuration).SetRelative().SetEase(birdRiseEase));

        if (soundSource && soundSource.clip)
        {
            soundSource.Play();
            seq.AppendInterval(soundSource.clip.length);
        }
        
        seq.AppendCallback(() => Destroy(gameObject));
    }

    private Sequence Fade(Transform t)
    {
        var tweens = t
            .GetComponentsInChildren<Renderer>()
            .SelectMany(r => r.materials)
            .Select(m => m.DOFade(0f, animationDuration));

        var seq = DOTween.Sequence();
        foreach (var tween in tweens) seq.Join(tween);

        return seq;
    }
}
