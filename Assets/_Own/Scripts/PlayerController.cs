using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Assertions;

public class PlayerController : GameplayObject
{
    [SerializeField] float speedForward = 0.5f;
    [SerializeField] float jumpPower = 2f;
    [SerializeField] float jumpDuration = 0.2f;
    [Space]
    [SerializeField] float enemyJumpOnErrorTolerance = 0.1f;

    enum InputKind
    {
        None,
        JumpLeft,
        JumpRight,
        JumpForward
    }

    private float speedMultiplier = 1.0f;
    private InputKind currentInput;
    private bool isJumping;
    private bool canFall = true;
    private Player player;

    protected override void Start()
    {
        base.Start();
        Assert.IsNotNull(currentLane);
        player = GetComponent<Player>();
    }
    
    void FixedUpdate()
    {
        transform.position += Vector3.forward * speedForward * player.GetSpeedMultiplier() * Time.fixedDeltaTime;

        UpdateBounds();

        if (!isJumping)
        {
            if (canFall && CheckDeath())
                return;

            Lane targetLane = GetTargetJumpLane();
            if (targetLane == null) return;
            
            Vector3 targetPosition = targetLane.GetJumpDestinationFrom(transform.position);
            JumpTo(targetPosition, targetLane);
        }
    }

    void Update()
    {
        Vector2 input = new Vector2(
            Input.GetAxis("Horizontal"),
            Input.GetAxis("Vertical")
        );

        if      (input.x >  0.01f) currentInput = InputKind.JumpRight;
        else if (input.x < -0.01f) currentInput = InputKind.JumpLeft;
        else if (input.y >  0.01f) currentInput = InputKind.JumpForward;
        else currentInput = InputKind.None;
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
        foreach (var obj in WorldRepresentation.Instance.objects)
        {
            if (obj.lane != currentLane) continue;

            if (obj.kind == ObjectKind.Platform && obj.IsInside(positionOnLane))
                return false;
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

    public void JumpTo(Vector3 targetPosition, Lane targetLane = null)
    {
        targetPosition.z += speedForward * jumpDuration;

        if (targetLane != null)
        {
            float laneTargetPosition = targetLane.GetPositionOnLane(targetPosition);
            ObjectRepresentation enemyRecord = WorldRepresentation.Instance.CheckByKind(ObjectKind.Enemy, targetLane, laneTargetPosition, enemyJumpOnErrorTolerance);
            enemyRecord?.gameObject.GetComponent<Enemy>().JumpedOn();
        }
        
        transform
            .DOJump(targetPosition, jumpPower, 1, jumpDuration)
            .OnComplete(() => isJumping = false);

        isJumping = true;

        if (targetLane != null)
            currentLane = targetLane;
    }

    public bool IsJumping()
    {
        return isJumping;
    }

    public void SetFall(bool canFall)
    {
        this.canFall = canFall;
    }
}