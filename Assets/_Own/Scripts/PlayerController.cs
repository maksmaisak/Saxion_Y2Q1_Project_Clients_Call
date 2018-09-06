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
    [SerializeField] new Rigidbody rigidbody;
    [SerializeField] Lane _currentLane;

    private Lane _startingLane;

    enum InputKind
    {
        None,
        JumpLeft,
        JumpRight,
        JumpForward
    }

    private InputKind currentInput;
    private bool isJumping;

    public Lane currentLane => _currentLane;
    public float positionOnLane => _currentLane.GetPositionOnLane(transform.position);

    void Start()
    {
        Assert.IsNotNull(rigidbody);
        Assert.IsNotNull(_currentLane);

        _startingLane = _currentLane;

        PlayerDeath deathScript = GetComponent<PlayerDeath>();
        if (deathScript != null)
            deathScript.HandleGameOver += HandleGameOver;
    }
    
    void FixedUpdate()
    {
        transform.position += Vector3.forward * speedForward * Time.fixedDeltaTime;

        if (!isJumping)
        {
            if (CheckDeath())
                return;

            Lane targetLane = GetTargetJumpLane();
            if (targetLane == null) return;

            Vector3 targetPosition = targetLane.GetJumpDestinationFrom(transform.position);
            targetPosition.z += jumpDuration * speedForward;
            transform
                .DOJump(targetPosition, jumpPower, 1, jumpDuration)
                .OnComplete(() => isJumping = false);

            isJumping = true;
            _currentLane = targetLane;
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

    private void HandleGameOver()
    {
        _currentLane = _startingLane;
    }
}