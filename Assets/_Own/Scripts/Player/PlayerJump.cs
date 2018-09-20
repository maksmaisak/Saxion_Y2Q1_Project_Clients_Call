using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Assertions;

public class PlayerJump
{
    private readonly Transform transform;
    private readonly Vector3 originalPosition;
    private readonly float jumpPower;
    private readonly float jumpDuration;
    
    public Vector3 targetPosition { get; private set; }

    private Action onCompleteCallback;
    private Tween jumpTween;
    
    public PlayerJump(Transform transform, Vector3 targetPosition, float jumpPower, float jumpDuration)
    {
        this.transform = transform;
        this.originalPosition = transform.position;
        this.jumpPower = jumpPower;
        this.jumpDuration = jumpDuration;

        this.targetPosition = targetPosition;

        jumpTween = MakeJumpTween();
    }

    public PlayerJump OnComplete(Action callback)
    {
        onCompleteCallback = callback;
        jumpTween?.OnComplete(new TweenCallback(callback));

        return this;
    }

    public PlayerJump ChangeTargetPosition(Vector3 newTargetPosition)
    {
        Assert.IsNotNull(jumpTween);

        if (jumpTween.IsComplete()) return this;

        float fullPosition = jumpTween.fullPosition;
        jumpTween = MakeJumpTween();
        jumpTween.fullPosition = fullPosition;

        return this;
    }

    private Tween MakeJumpTween()
    {
        transform.position = originalPosition;
        var tween = transform.DOJump(targetPosition, jumpPower, 1, jumpDuration);
        
        if (onCompleteCallback != null)
        {
            tween.OnComplete(new TweenCallback(onCompleteCallback));
        }

        return tween;
    }
}