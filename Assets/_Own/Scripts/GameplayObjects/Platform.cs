using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using DG.Tweening;

public class Platform : GameplayObject
{
    [Space]
    [SerializeField] bool movingPlatform;
    [SerializeField] float moveInterval = 0.6f;
    [SerializeField] float shakeDuration = 0.4f;
    [SerializeField] float moveDuration = 0.2f;
    [SerializeField] bool isMovingRight;
    [Header("Animation")] 
    [SerializeField] float shakeStrength = 10f;
    [SerializeField] int shakeVibrato = 10;
    [SerializeField] float shakeElasticity = 1f;

    private float moveCountdown;
    private bool isPlayingAnimation;
    private bool isPlayerCloseToThis;

    protected override void Start()
    {
        base.Start();
        moveCountdown = moveInterval;
    }

    void FixedUpdate()
    {
        if (!movingPlatform) return;
        if (isPlayingAnimation) return;
        
        if (moveCountdown > 0f)
        {
            moveCountdown -= Time.fixedDeltaTime;
            if (moveCountdown > 0f) return;

            ShakeAndMove();
        }
    }

    private void ShakeAndMove()
    {
        UpdateMovementDirection();

        isPlayingAnimation = true;
        PlayShake().OnComplete(() =>
        {
            StartCoroutine(MoveCoroutine());
        });
    }

    private void UpdateMovementDirection()
    {
        Lane targetLane = isMovingRight ? currentLane.rightNeighbor : currentLane.leftNeighbor;
        if (targetLane == null) isMovingRight = !isMovingRight;
    }

    private Tween PlayShake()
    {
        Vector3 targetAngles = Vector3.forward * shakeStrength * (isMovingRight ? -1f : 1f);
        return transform.DOPunchRotation(targetAngles, shakeDuration, shakeVibrato, shakeElasticity);
    }
    
    private IEnumerator MoveCoroutine()
    {
        yield return new WaitUntil(() => LevelState.instance.canPlatformsMove);
        MoveTo(GetNextTargetLane());
    }

    private Lane GetNextTargetLane()
    {
        UpdateMovementDirection();
        return isMovingRight ? currentLane.rightNeighbor : currentLane.leftNeighbor;
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
                representation.location.laneA = targetLane;
                representation.location.laneB = null;
                representation.location.isMovingBetweenLanes = false;

                isPlayingAnimation = false;
                moveCountdown = moveInterval;
            });

        representation.location.laneB = targetLane;
        representation.location.isMovingBetweenLanes = true;
    }
}