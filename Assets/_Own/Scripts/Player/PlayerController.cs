﻿using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Assertions;

public class PlayerController : GameplayObject
{
    [Space]
    [SerializeField] float baseSpeed = 10f;
    [SerializeField] float jumpPower = 2f;
    [SerializeField] float jumpDuration = 0.2f;

    [Tooltip("Smaller means the player can double jump within a larger period of time. Since the previous jump")]
    [SerializeField] float doubleJumpTime = 0.1f;
    [Space] 
    [SerializeField] float enemyJumpOnErrorTolerance = 0.1f;
    [SerializeField] float platformErrorTolerance = 0.1f;
    [SerializeField] float obstacleCollisionTolerance = 0.1f;

    [Tooltip("Smaller is faster. Difference gets multiplied by this every second.")] 
    [SerializeField] float platformSnappingCoefficient = 0.01f;
    [Space] 
    [SerializeField] ObjectRepresentation currentPlatformRepresentation;
    [Space]
    [SerializeField] AudioSource jumpAudioSource;

    enum InputKind
    {
        None,
        JumpLeft,
        JumpRight,
        JumpForward
    }

    private Player player;
    private new Rigidbody rigidbody;

    private bool isJumpReleased = true;
    private bool isJumpPressedDuringJump = false;
    private bool isDoubleJump = false;
    private float previousJumpStartTime = 0f;
    private InputKind currentInput;

    public bool isJumping { get; private set; }

    private float currentSpeed => baseSpeed * player.GetSpeedMultiplier();

    protected override void Start()
    {
        base.Start();
        Assert.IsNotNull(currentLane);
        player = GetComponent<Player>();
        rigidbody = GetComponent<Rigidbody>();
        Assert.IsNotNull(player);
        Assert.IsNotNull(rigidbody);
    }

    void Update()
    {
        UpdateInput();
                
        if (!isJumping)
        {
            transform.position += Vector3.forward * currentSpeed * Time.deltaTime;
            UpdateBounds();
            UpdateCurrentPlatform();
            SnapToPlatform();
            
            if (!player.isGodMode && CheckDeath())
                return;

            Lane targetLane = GetTargetJumpLane();
            if (targetLane == null) return;

            JumpTo(targetLane);
        }
        else
        {
            if (isJumpPressedDuringJump)
            {
                isDoubleJump = true;
                isJumpPressedDuringJump = false;
            }

            UpdateBounds();
            UpdateCurrentPlatform();
        }
    }

    public void JumpTo(Lane targetLane)
    {
        Vector3 targetPosition;
        if (targetLane != null)
        {
            targetPosition = targetLane.GetJumpDestinationFrom(transform.position);
            targetPosition.z += currentSpeed * jumpDuration;
            KillEnemyAtJumpDestination(targetLane, targetPosition);
        }
        else
        {
            // Temp
            targetPosition = transform.position + Vector3.right * 3f;
            targetPosition.z += currentSpeed * jumpDuration;
        }

        transform
            .DOJump(targetPosition, jumpPower, 1, jumpDuration)
            .OnComplete(() =>
            {
                isJumping = false;
                representation.location.laneA = targetLane;
                representation.location.laneB = null;
                representation.location.isMovingBetweenLanes = false;
                representation.location.isAboveLane = false;

                if (isDoubleJump)
                    JumpTo(targetLane);
            });

        isJumping = true;
        if (isDoubleJump)
            isDoubleJump = false;
        isJumpReleased = false;
        previousJumpStartTime = Time.time;
        representation.location.laneB = targetLane;
        representation.location.isMovingBetweenLanes = currentLane && currentLane != targetLane;
        representation.location.isAboveLane = true;

        PlayJumpSound();
    }

    private void PlayJumpSound()
    {
        if (!jumpAudioSource || !jumpAudioSource.clip) return;
        
        jumpAudioSource.Play();
        jumpAudioSource.pitch = jumpAudioSource.clip.length / jumpDuration;
    }

    private void UpdateCurrentPlatform()
    {
        if (currentLane == null)
        {
            currentPlatformRepresentation = null;
        }
        else
        {
            currentPlatformRepresentation = WorldRepresentation.Instance.CheckByKind(
                ObjectKind.Platform, currentLane, positionOnLane, platformErrorTolerance, areMovingObjectsAllowed: true
            );
        }

        if (currentPlatformRepresentation != null)
        {
            representation.location.laneA = GetClosestLaneFrom(currentPlatformRepresentation.location);
        }
    }

    private Lane GetClosestLaneFrom(ObjectLocation location)
    {
        if (!location.laneB) return location.laneA;
        return DistanceTo(location.laneB) < DistanceTo(location.laneA) ? location.laneB : location.laneA;
    }

    private void SnapToPlatform()
    {
        if (currentPlatformRepresentation == null) return;

        Vector3 targetPosition = currentPlatformRepresentation.gameObject.transform.position;
        Vector3 newPosition = transform.position;
        newPosition.x = Mathf.Lerp(
            newPosition.x, targetPosition.x,
            1f - Mathf.Pow(platformSnappingCoefficient, Time.deltaTime)
        );
        
        transform.position = newPosition;
    }

    private bool CheckDeath()
    {
        return CheckDeathFall() || CheckDeathObstaclesEnemies();
    }
    
    private bool CheckDeathFall()
    {
        if (currentPlatformRepresentation != null) return false;
        
        GetComponent<PlayerDeath>().DeathFall();
        return true;
    }

    private bool CheckDeathObstaclesEnemies()
    {
        PlayerDeath playerDeath = GetComponent<PlayerDeath>();
        
        var obj = WorldRepresentation.Instance.CheckIntersect(representation, ObjectKind.Obstacle | ObjectKind.Enemy, -obstacleCollisionTolerance);
        if (obj == null) return false;
        
        if (obj.kind == ObjectKind.Enemy)
        {
            playerDeath.DeathEnemy();
        }
        else if(obj.kind == ObjectKind.Obstacle)
        {
            // Obstacles behind the player don't count.
            if (obj.location.bounds.middle < positionOnLane) return false;
            playerDeath.DeathObstacle();
        }

        return true;
    }

    private Lane GetTargetJumpLane()
    {
        switch (currentInput)
        {
            case InputKind.JumpLeft: return currentLane.leftNeighbor;
            case InputKind.JumpRight: return currentLane.rightNeighbor;
            case InputKind.JumpForward: return currentLane;
        }

        return null;
    }

    private void KillEnemyAtJumpDestination(Lane targetLane, Vector3 targetPosition)
    {
        float laneTargetPosition = targetLane.GetPositionOnLane(targetPosition);
        ObjectRepresentation enemyRecord = WorldRepresentation.Instance.CheckByKind(ObjectKind.Enemy, targetLane,
            laneTargetPosition, enemyJumpOnErrorTolerance);
        
        enemyRecord?.gameObject.GetComponent<Enemy>().JumpedOn();
    }

    private void UpdateInput()
    {
        Vector2 input = new Vector2(
            Input.GetAxis("Horizontal"),
            Input.GetAxis("Vertical")
        );

        if (input.x > 0.01f) currentInput = InputKind.JumpRight;
        else if (input.x < -0.01f) currentInput = InputKind.JumpLeft;
        else if (input.y > 0.01f && isJumpReleased) currentInput = InputKind.JumpForward;
        else currentInput = InputKind.None;

        if (isDoubleJump || isJumpPressedDuringJump)
            return;

        /// If the JumpForward button was released
        /// and pressed again right before the previous jump ends
        /// then the character should make a double jump
        bool isHoldingJump = Input.GetKey(KeyCode.W);
        if (isJumping)
        {
            if (!isJumpReleased && !isHoldingJump)
                isJumpReleased = true;

            if (isJumpReleased && Input.GetKeyDown(KeyCode.W))
            {
                float timeNow = Time.time;
                float nextLandingTime = previousJumpStartTime + jumpDuration;
                if (timeNow >= previousJumpStartTime + doubleJumpTime && timeNow <= nextLandingTime)
                    isJumpPressedDuringJump = true;
            }
        }
        else if (!isJumpReleased && !isHoldingJump)
            isJumpReleased = true;

        if (!isJumpReleased)
            currentInput = InputKind.None;
    }
}