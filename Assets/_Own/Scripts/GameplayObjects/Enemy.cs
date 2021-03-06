﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using DG.Tweening;

public class Enemy : GameplayObject
{
    [Space]
    [SerializeField] float jumpInterval = 1f;
    [SerializeField] float jumpPower = 2f;
    [SerializeField] float jumpDuration = 0.2f;
    [Space] 
    [SerializeField] int scoreBonusWhenKilled = 100;
    [SerializeField] Animator animator;
    [SerializeField] AudioSource deathAudioSource;
    
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

            if (!deathAudioSource || !deathAudioSource) return;

            deathAudioSource.Play();
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
        UpdateMovementDirection();
        animator.SetTrigger(isJumpingRight ? "Jump_Right" : "Jump_Left");
        
        JumpTo(GetNextTargetLane());
        jumpCountdown = jumpInterval;
    }
    
    private void UpdateMovementDirection()
    {
        Lane targetLane = isJumpingRight ? currentLane.rightNeighbor : currentLane.leftNeighbor;
        if (targetLane == null) isJumpingRight = !isJumpingRight;
    }

    private Lane GetNextTargetLane()
    {
        UpdateMovementDirection();
        return isJumpingRight ? currentLane.rightNeighbor : currentLane.leftNeighbor;
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
            .Append(transform.DOJump(new Vector3(1.5f, 0f, 10f), 1f, 1, 0.5f).SetRelative())
            .Append(transform.DOMoveY(-20f, 1f).SetRelative().SetEase(Ease.InExpo))
            .AppendCallback(() => Destroy(gameObject));*/
    }
}