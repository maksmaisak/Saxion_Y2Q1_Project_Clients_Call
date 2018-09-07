using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using DG.Tweening;

public class Enemy : GameplayObject
{
    [SerializeField] float jumpInterval = 1f;
    [SerializeField] float jumpPower = 2f;
    [SerializeField] float jumpDuration = 0.2f;

    private float jumpCountdown;
    private bool isJumpingRight;

    private bool isJumping;
    private bool wasJumpedOn;

    void FixedUpdate()
    {
        if (isJumping) return;

        if (wasJumpedOn)
        {
            PlayDeathAnimation();
            RemoveFromWorldModel();
            enabled = false;
            return;
        }

        if (jumpCountdown > 0f)
        {
            jumpCountdown -= Time.fixedDeltaTime;
            if (jumpCountdown > 0f) return;
        }

        Jump();
    }

    public void JumpedOn()
    {
        wasJumpedOn = true;
    }

    private void Jump()
    {
        JumpTo(GetNextTargetLane());
        jumpCountdown = jumpInterval;
    }

    private Lane GetNextTargetLane()
    {
        Lane targetLane = isJumpingRight ? currentLane.rightNeighbor : currentLane.leftNeighbor;
        if (targetLane != null) return targetLane;
        
        isJumpingRight = !isJumpingRight;
        targetLane = isJumpingRight ? currentLane.rightNeighbor : currentLane.leftNeighbor;
        return targetLane;
    }
    
    private void JumpTo(Lane targetLane)
    {
        Assert.IsNotNull(targetLane);
        
        Vector3 targetPosition = targetLane.GetJumpDestinationFrom(transform.position);
        transform
            .DOJump(targetPosition, jumpPower, 1, jumpDuration)
            .OnComplete(() =>
            {
                isJumping = false;
                currentLane = targetLane;
            });

        isJumping = true;
        currentLane = null;
    }
    
    private void PlayDeathAnimation()
    {
        DOTween
            .Sequence()
            .Append(transform.DOJump(Vector3.down , 1f, 1, 0.5f).SetRelative())
            .Append(transform.DOJump(Vector3.right, 1f, 1, 0.5f).SetRelative())
            .AppendCallback(() => Destroy(gameObject));
    }
}