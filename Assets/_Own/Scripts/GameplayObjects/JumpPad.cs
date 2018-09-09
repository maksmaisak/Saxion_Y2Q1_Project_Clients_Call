using UnityEngine;
using DG.Tweening;
using UnityEngine.Assertions;

public class JumpPad : GameplayObject
{
    [SerializeField] Lane targetLane;
    [SerializeField] float playerDetectionRadius = 0.1f;

    void FixedUpdate()
    {
        ObjectRepresentation playerRepresentation = WorldRepresentation.Instance.CheckByKind(
            ObjectKind.Player, currentLane, positionOnLane, playerDetectionRadius, areMovingObjectsAllowed: false
        );
        if (playerRepresentation == null) return;
        if (playerRepresentation.location.isMovingBetweenLanes) return;

        /// TODO Meh. Fix direct dependency.
        playerRepresentation.gameObject.GetComponent<PlayerController>().JumpTo(targetLane);
        
        enabled = false;
    }
}