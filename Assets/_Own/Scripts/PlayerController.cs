using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Assertions;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float speedForward = 0.5f;
    [SerializeField] float jumpPower = 2f;
    [SerializeField] float jumpDuration = 0.2f;
    [Space] 
    [SerializeField] Lane _currentLane;

    private Lane _startingLane;

    enum InputKind
    {
        None,
        JumpLeft,
        JumpRight,
        JumpForward
    }

    private float _oldSpeed;
    private InputKind currentInput;
    private bool isJumping;
    private bool canFall = true;

    public float positionOnLane => _currentLane.GetPositionOnLane(transform.position);

    void Start()
    {
        Assert.IsNotNull(_currentLane);

        _startingLane = _currentLane;
    }
    
    void FixedUpdate()
    {
        transform.position += Vector3.forward * speedForward * Time.fixedDeltaTime;

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
            if (obj.lane != _currentLane) continue;

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
            if (obj.lane != _currentLane) continue;

            if (obj.kind == ObjectKind.Platform && obj.IsInside(positionOnLane))
            {
                return false;
            }
        }

        return true;
    }

    private Lane GetTargetJumpLane()
    {
        switch (currentInput)
        {
            case InputKind.JumpLeft: return _currentLane.leftNeighbor;
            case InputKind.JumpRight: return _currentLane.rightNeighbor;
            case InputKind.JumpForward: return _currentLane;
        }

        return null;
    }

    public void JumpTo(Vector3 targetPosition, Lane targetLane = null)
    {
        targetPosition.z += jumpDuration * speedForward;

        if (targetLane != null)
        {
            float laneTargetPosition = targetLane.GetPositionOnLane(targetPosition);
            ObjectRepresentation enemyRecord = WorldRepresentation.Instance.CheckEnemy(targetLane, laneTargetPosition);
            enemyRecord?.gameObject.GetComponent<Enemy>().JumpedOn();
        }
        
        transform
            .DOJump(targetPosition, jumpPower, 1, jumpDuration)
            .OnComplete(() => isJumping = false);

        isJumping = true;

        if (targetLane != null)
            _currentLane = targetLane;
    }
    
    public bool IsJumping()
    {
        return isJumping;
    }

    public void SetFall(bool canFall)
    {
        this.canFall = canFall;
    }

    public void SetSpeed(float newSpeed)
    {
        _oldSpeed = speedForward;
        speedForward = newSpeed;
    }

    public float GetOldSpeed()
    {
        return _oldSpeed;
    }
}