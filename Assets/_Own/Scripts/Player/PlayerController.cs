using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Assertions;

[Serializable]
public struct PlayerDifficultySpeed
{
    public Difficulty difficulty;
    public float speed;
}

public class PlayerController : GameplayObject,
    IEventReceiver<OnLevelBeganSwitching>,
    IEventReceiver<OnPlayerDeath>,
    IEventReceiver<OnPlayerWillRespawn>,
    IEventReceiver<OnPlayerRespawned>
{
    [Space]
    [SerializeField] float jumpPower = 2f;
    [SerializeField] float jumpDuration = 0.2f;
    [Tooltip("Smaller means the player can double jump within a larger period of time. Since the previous jump")]
    [SerializeField] float doubleJumpTime = 0.2f;
    [Space]
    [SerializeField] float enemyJumpOnErrorTolerance = 0.1f;
    [SerializeField] float platformErrorTolerance = 0.1f;
    [SerializeField] float obstacleCollisionTolerance = 0.1f;
    [Space]
    [Tooltip("Smaller is faster. Distance gets multiplied by this every second.")] 
    [SerializeField] float platformSnappingCoefficient = 0.01f;
    [SerializeField] float minPlatformSnappingDistance = 1.5f;
    [Space] 
    [SerializeField] ObjectRepresentation currentPlatformRepresentation;
    [Space]
    [SerializeField] AudioSource jumpAudioSource;
    [SerializeField] Animator animator;
    [Space]
    [SerializeField] List<PlayerDifficultySpeed> playerDifficultySpeeds = new List<PlayerDifficultySpeed>();

    enum InputKind
    {
        None,
        JumpLeft,
        JumpRight,
        JumpForward
    }

    private Player player;

    private bool wasJumpPressedDuringJump = false;
    float baseSpeed = 10f;
    private float previousJumpStartTime = 0f;
    private InputKind currentInput;
    
    private PlayerJump currentJumpAnimation;
    public bool isJumping { get; private set; }
    public ObjectRepresentation previousPlatform { get; private set; }

    private float currentSpeed => baseSpeed * player.GetSpeedMultiplier();

    protected override void Start()
    {
        base.Start();
        Assert.IsTrue(playerDifficultySpeeds.Count >= 3);
        Assert.IsNotNull(currentLane);
        player = GetComponent<Player>();
        baseSpeed = playerDifficultySpeeds.FirstOrDefault(x => x.difficulty == GlobalState.instance.currentGameDifficulty).speed;
        Assert.IsNotNull(player);
        Assert.IsNotNull(animator);
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

            if (player.isGodMode || !CheckDeath())
            {
                Lane targetLane = GetTargetJumpLane();
                if (targetLane) JumpTo(targetLane);
            }
        }
        else
        {
            UpdateBounds();
            UpdateCurrentPlatform();
            SnapToMovingPlatformInMidair();
        }
    }

    void OnEnable()
    {
        currentInput = InputKind.None;
        wasJumpPressedDuringJump = false;
    }
    
    public void On(OnLevelBeganSwitching message) => enabled = false;
    public void On(OnPlayerDeath     message) => enabled = false;

    public void On(OnPlayerWillRespawn message) => ResetToPreviousPlatform();
    public void On(OnPlayerRespawned message) => enabled = true;

    public void ResetToPreviousPlatform()
    {
        animator.Rebind();

        currentPlatformRepresentation = previousPlatform;
        representation.location.laneA = GetClosestLaneFrom(currentPlatformRepresentation.location);
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

        currentJumpAnimation = new PlayerJump(transform, targetPosition, jumpPower, jumpDuration)
            .OnComplete(() =>
            {
                currentJumpAnimation = null;
                isJumping = false;

                representation.location.laneA = targetLane;
                representation.location.laneB = null;
                representation.location.isMovingBetweenLanes = false;
                representation.location.isAboveLane = false;
            });

        isJumping = true;
        wasJumpPressedDuringJump = false;
        previousJumpStartTime = Time.unscaledTime;
        if (currentPlatformRepresentation != null)
            previousPlatform = currentPlatformRepresentation;
        representation.location.laneB = targetLane;
        representation.location.isMovingBetweenLanes = currentLane && currentLane != targetLane;
        representation.location.isAboveLane = true;

        PlayJumpSound();
        
        if      (targetLane == currentLane.leftNeighbor ) animator.SetTrigger("Jump_L");
        else if (targetLane == currentLane.rightNeighbor) animator.SetTrigger("Jump_R");
        else if (targetLane == currentLane) animator.SetTrigger("Jump_F");
        else animator.SetTrigger("Jump_R"); // TEMP. Jump to a null lane is right by default.
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
            currentPlatformRepresentation = LevelState.instance.CheckByKind(
                ObjectKind.Platform, currentLane, positionOnLane, platformErrorTolerance, areMovingObjectsAllowed: true
            );
        }

        if (currentPlatformRepresentation != null && !IsCloseEnoughForSnapping(currentPlatformRepresentation))
        {
            currentPlatformRepresentation = null;
        }

        if (currentPlatformRepresentation != null)
        {
            previousPlatform = currentPlatformRepresentation;
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
        if (!IsCloseEnoughForSnapping(currentPlatformRepresentation)) return;
        
        Vector3 currentPlatformPosition = currentPlatformRepresentation.gameObject.transform.position;
        Vector3 newPosition = transform.position;
        newPosition.x = Mathf.Lerp(
            newPosition.x, currentPlatformPosition.x,
            1f - Mathf.Pow(platformSnappingCoefficient, Time.deltaTime)
        );
        
        transform.position = newPosition;
    }

    private bool IsCloseEnoughForSnapping(ObjectRepresentation platformRepresentation)
    {
        return Mathf.Abs(transform.position.x - platformRepresentation.gameObject.transform.position.x) <= minPlatformSnappingDistance;
    }

    private void SnapToMovingPlatformInMidair()
    {
        if (!isJumping) return;
        if (currentPlatformRepresentation == null) return;
        if (!currentPlatformRepresentation.location.isMovingBetweenLanes) return;
        if (!IsCloseEnoughForSnapping(currentPlatformRepresentation)) return;
        
        Vector3 targetPosition = currentJumpAnimation.targetPosition;
        Vector3 currentPlatformPosition = currentPlatformRepresentation.gameObject.transform.position;
        targetPosition.x = currentPlatformPosition.x;
        currentJumpAnimation?.ChangeTargetPosition(targetPosition);
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
        var obj = LevelState.instance.CheckIntersect(representation, ObjectKind.Obstacle | ObjectKind.Enemy, -obstacleCollisionTolerance);
        if (obj == null) return false;
        
        PlayerDeath playerDeath = GetComponent<PlayerDeath>();
        
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
        ObjectRepresentation enemyRecord = LevelState.instance.CheckByKind(ObjectKind.Enemy, targetLane,
            laneTargetPosition, enemyJumpOnErrorTolerance);
        
        enemyRecord?.gameObject.GetComponent<Enemy>().JumpedOn();
    }

    private void UpdateInput()
    {
        var input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        bool wasJumpPressed = Input.GetButtonDown("Horizontal") || Input.GetButtonDown("Vertical");

        if (!wasJumpPressed)
        {
            if (!wasJumpPressedDuringJump) currentInput = InputKind.None;
            return;
        }
        
        if (input.x > 0.01f) currentInput = InputKind.JumpRight;
        else if (input.x < -0.01f) currentInput = InputKind.JumpLeft;
        else if (input.y > 0.01f) currentInput = InputKind.JumpForward;

        /// If the Jump button was pressed
        /// right before the previous jump ends
        /// then the character should make another jump after the current one ends.
        if (isJumping)
        {
            float timeNow = Time.unscaledTime;
            float nextLandingTime = previousJumpStartTime + jumpDuration;
            if (timeNow >= nextLandingTime - doubleJumpTime && timeNow <= nextLandingTime)
                wasJumpPressedDuringJump = true;
        }
    }
}