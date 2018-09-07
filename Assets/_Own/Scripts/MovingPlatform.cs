using UnityEngine;
using UnityEngine.Assertions;
using DG.Tweening;

public class MovingPlatform : GameplayObject
{
    [SerializeField] float moveInterval = 1f;
    [SerializeField] float moveDuration = 0.2f;

    private float moveCountdown;
    private bool isMovingRight;

    private bool isMoving;
    
    void FixedUpdate()
    {
        if (isMoving) return;

        if (moveCountdown > 0f)
        {
            moveCountdown -= Time.fixedDeltaTime;
            if (moveCountdown > 0f) return;
        }

        Move();
    }
    
    private void Move()
    {
        JumpTo(GetNextTargetLane());
        moveCountdown = moveInterval;
    }

    private Lane GetNextTargetLane()
    {
        Lane targetLane = isMovingRight ? currentLane.rightNeighbor : currentLane.leftNeighbor;
        if (targetLane != null) return targetLane;
        
        isMovingRight = !isMovingRight;
        targetLane = isMovingRight ? currentLane.rightNeighbor : currentLane.leftNeighbor;
        return targetLane;
    }
    
    private void JumpTo(Lane targetLane)
    {
        Assert.IsNotNull(targetLane);
        
        Vector3 targetPosition = targetLane.GetJumpDestinationFrom(transform.position);
        transform
            .DOMove(targetPosition, moveDuration)
            .OnComplete(() =>
            {
                isMoving = false;
                currentLane = targetLane;
                representation.destinationLane = null;
            });

        isMoving = true;
        representation.destinationLane = targetLane;
    }
}