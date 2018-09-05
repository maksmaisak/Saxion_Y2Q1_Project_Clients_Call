﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using DG.Tweening;

public class Enemy : MonoBehaviour
{
    [SerializeField] Lane currentLane;
    [SerializeField] float jumpInterval = 1f;
    [SerializeField] float jumpPower = 2f;
    [SerializeField] float jumpDuration = 0.2f;

    private float jumpCountdown;
    private bool isJumpingRight;

    private bool isJumping;

    void Start()
    {
        Assert.IsNotNull(currentLane);
    }

    void FixedUpdate()
    {
        if (isJumping) return;
        
        if (jumpCountdown > 0f)
        {
            jumpCountdown -= Time.fixedDeltaTime;
            if (jumpCountdown > 0f) return;
        }

        Jump();
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
            .OnComplete(() => isJumping = false);

        isJumping = true;
        currentLane = targetLane;
    }
}