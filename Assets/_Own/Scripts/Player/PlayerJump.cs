using System;
using DG.Tweening;
using UnityEngine;

public class PlayerJump
{
    private readonly Transform transform;
    private readonly float jumpPower;
    private readonly float jumpDuration;
    
    private Vector3 targetPosition;

    private Action onCompleteCallback;

    private Tween jumpTween;
    
    public PlayerJump(Transform transform, Vector3 targetPosition, float jumpPower, float jumpDuration)
    {
        this.transform = transform;
        this.jumpPower = jumpPower;
        this.jumpDuration = jumpDuration;

        this.targetPosition = targetPosition;

        jumpTween = transform
            .DOJump(targetPosition, jumpPower, 1, jumpDuration);
    }

    public PlayerJump OnComplete(Action callback)
    {
        onCompleteCallback = callback;
        jumpTween?.OnComplete(new TweenCallback(callback));

        return this;
    }
}