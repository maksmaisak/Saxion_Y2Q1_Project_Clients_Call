using UnityEngine;
using DG.Tweening;
using UnityEngine.Assertions;

public class JumpPad : GameplayObject
{
    [SerializeField] Lane targetLane;
    [SerializeField] float playerDetectionMargin = 0.1f;

    void FixedUpdate()
    {
        ObjectRepresentation playerRepresentation = WorldRepresentation.Instance.CheckIntersect(
            representation, ObjectKind.Player, playerDetectionMargin
        );
        if (playerRepresentation == null) return;
        if (playerRepresentation.location.isMovingBetweenLanes) return;

        /// TODO Meh. Fix direct dependency. Something like new JumpToLane(taretLane).PostEvent()?
        playerRepresentation.gameObject.GetComponent<PlayerController>().JumpTo(targetLane);
        
        enabled = false;
    }
}