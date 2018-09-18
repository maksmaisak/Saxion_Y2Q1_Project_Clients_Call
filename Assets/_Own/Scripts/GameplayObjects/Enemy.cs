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
    [Space] 
    [SerializeField] int scoreBonusWhenKilled = 100;
    [SerializeField] Animator animator;
    
    private float jumpCountdown;
    private bool isJumpingRight;

    private bool isJumping;
    private bool wasJumpedOn;

    protected override void Start()
    {
        base.Start();
        
        if (!animator) animator = GetComponentInChildren<Animator>();
        Assert.IsNotNull(animator); 
    }

    void FixedUpdate()
    {
        if (isJumping) return;

        if (wasJumpedOn)
        {
            PlayDeathAnimation();
            RemoveFromLevelState();
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
        new OnEnemyKilled().PostEvent();
        new OnScoreChange(scoreBonusWhenKilled).PostEvent();
    }

    private void Jump()
    {
        JumpTo(GetNextTargetLane());
        jumpCountdown = jumpInterval;
        
        animator.SetTrigger("Jump");
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
                representation.location.laneA = targetLane;
                representation.location.laneB = null;
                representation.location.isMovingBetweenLanes = false;
                representation.location.isAboveLane = false;
            });
        
        isJumping = true;
        representation.location.laneB = targetLane;
        representation.location.isMovingBetweenLanes = currentLane && currentLane != targetLane;
        representation.location.isAboveLane = true;
    }
    
    private void PlayDeathAnimation()
    {
        animator.SetTrigger("Death");
        
        /*DOTween
            .Sequence()
            .Append(transform.DOJump(Vector3.down , 1f, 1, 0.5f).SetRelative())
            .Append(transform.DOJump(Vector3.right, 1f, 1, 0.5f).SetRelative())
            .AppendCallback(() => Destroy(gameObject));*/
    }
}