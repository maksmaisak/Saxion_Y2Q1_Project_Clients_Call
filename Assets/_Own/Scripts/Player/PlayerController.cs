using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Assertions;

public class PlayerController : GameplayObject
{
    [SerializeField] float baseSpeed = 10f;
    [SerializeField] float jumpPower = 2f;
    [SerializeField] float jumpDuration = 0.2f;
    [Space] [SerializeField] float enemyJumpOnErrorTolerance = 0.1f;
    [SerializeField] float platformErrorTolerance = 0.1f;

    [Tooltip("Smaller is faster. Difference gets multiplied by this every second.")] [SerializeField]
    float platformSnappingCoefficient = 0.01f;

    [Space] [SerializeField] ObjectRepresentation currentPlatformRepresentation;

    enum InputKind
    {
        None,
        JumpLeft,
        JumpRight,
        JumpForward
    }

    private Player player;

    private InputKind currentInput;
    public bool isJumping { get; private set; }

    private float currentSpeed => baseSpeed * player.GetSpeedMultiplier();

    protected override void Start()
    {
        base.Start();
        Assert.IsNotNull(currentLane);
        player = GetComponent<Player>();
    }

    void FixedUpdate()
    {
        transform.position += Vector3.forward * currentSpeed * Time.fixedDeltaTime;

        UpdateBounds();

        UpdateCurrentPlatform();
        SnapToPlatform();

        if (!isJumping)
        {
            if (!player.isGodMode && CheckDeath())
                return;

            Lane targetLane = GetTargetJumpLane();
            if (targetLane == null) return;

            JumpTo(targetLane);
        }
    }

    void Update()
    {
        Vector2 input = new Vector2(
            Input.GetAxis("Horizontal"),
            Input.GetAxis("Vertical")
        );

        if (input.x > 0.01f) currentInput = InputKind.JumpRight;
        else if (input.x < -0.01f) currentInput = InputKind.JumpLeft;
        else if (input.y > 0.01f) currentInput = InputKind.JumpForward;
        else currentInput = InputKind.None;
    }

    private void UpdateCurrentPlatform()
    {
        currentPlatformRepresentation = WorldRepresentation.Instance.CheckByKind(
            ObjectKind.Platform, currentLane, positionOnLane, platformErrorTolerance, areMovingObjectsAllowed: true
        );

        // Might move you aside if the platform is moving while you jump on it.
        transform.SetParent(currentPlatformRepresentation?.gameObject.transform);

        if (currentPlatformRepresentation != null)
        {
            currentLane = currentPlatformRepresentation.destinationLane ?? currentPlatformRepresentation.lane;
        }
    }

    private void SnapToPlatform()
    {
        if (currentPlatformRepresentation == null) return;

        Vector3 localPosition = transform.localPosition;
        localPosition.x *= Mathf.Pow(platformSnappingCoefficient, Time.deltaTime);
        transform.localPosition = localPosition;
    }

    private bool CheckDeath()
    {
        PlayerDeath playerDeath = GetComponent<PlayerDeath>();

        foreach (var obj in WorldRepresentation.Instance.objects)
        {
            if (obj.lane != currentLane) continue;

            bool isObstacle = obj.kind == ObjectKind.Obstacle;
            bool isEnemy = obj.kind == ObjectKind.Enemy;
            if ((isObstacle || isEnemy) && obj.IsInside(positionOnLane))
            {
                if (isObstacle)
                {
                    playerDeath.DeathObstacle();
                }
                else
                {
                    playerDeath.DeathEnemy();
                }

                return true;
            }
        }

        if (IsFalling())
        {
            playerDeath.DeathFall();
            return true;
        }

        return false;
    }

    private bool IsFalling()
    {
        return currentPlatformRepresentation == null;
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

    public void JumpTo(Lane targetLane)
    {
        Vector3 targetPosition = targetLane.GetJumpDestinationFrom(transform.position);
        targetPosition.z += currentSpeed * jumpDuration;

        // Check if will kill enemy at target position
        KillEnemyAtJumpDestination(targetLane, targetPosition);

        transform
            .DOJump(targetPosition, jumpPower, 1, jumpDuration)
            .OnComplete(() =>
            {
                isJumping = false;
                currentLane = targetLane;
                representation.destinationLane = null;
            });

        isJumping = true;
        representation.destinationLane = targetLane;
    }

    private void KillEnemyAtJumpDestination(Lane targetLane, Vector3 targetPosition)
    {
        float laneTargetPosition = targetLane.GetPositionOnLane(targetPosition);
        ObjectRepresentation enemyRecord = WorldRepresentation.Instance.CheckByKind(ObjectKind.Enemy, targetLane,
            laneTargetPosition, enemyJumpOnErrorTolerance);
        enemyRecord?.gameObject.GetComponent<Enemy>().JumpedOn();
    }
}