using UnityEngine;
using UnityEngine.Assertions;
using DG.Tweening;

public class Platform : GameplayObject
{
    [Space]
    [SerializeField] bool movingPlatform;
    [SerializeField] float moveInterval = 1f;
    [SerializeField] float moveDuration = 0.2f;
    [SerializeField] bool isMovingRight;
    
    private float moveCountdown;
    private bool isMoving;
    
    void FixedUpdate()
    {
        if (!movingPlatform) return;
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
        MoveTo(GetNextTargetLane());
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
    
    private void MoveTo(Lane targetLane)
    {
        Assert.IsNotNull(targetLane);
        
        Vector3 targetPosition = targetLane.GetJumpDestinationFrom(transform.position);
        transform
            .DOMove(targetPosition, moveDuration)
            .SetEase(Ease.InOutQuart)
            .OnComplete(() =>
            {
                isMoving = false;
                representation.location.laneA = targetLane;
                representation.location.laneB = null;
            });

        isMoving = true;
        representation.location.laneB = targetLane;
    }
}