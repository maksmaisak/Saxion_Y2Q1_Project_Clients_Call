using UnityEngine;
using DG.Tweening;
using UnityEngine.Assertions;

public class JumpPad : GameplayObject
{
    [SerializeField] Lane targetLane;
    [SerializeField] float playerDetectionRadius = 0.1f;

    /// TODO Maybe just have a destination lane specifiable instead of all that ^. As in "send player to given lane".

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        Assert.IsNotNull(targetLane);
    }

    void FixedUpdate()
    {
        ObjectRepresentation playerRepresentation = WorldRepresentation.Instance.CheckByKind(
            ObjectKind.Player, currentLane, positionOnLane, playerDetectionRadius, areMovingObjectsAllowed: false
        );
        if (playerRepresentation == null) return;

        Debug.Log("x");
        /// TODO Fix direct dependency.
        playerRepresentation.gameObject.GetComponent<PlayerController>().JumpTo(targetLane);
    }

    /*private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        var controller = other.GetComponent<PlayerController>();
        Debug.Assert(controller != null);

        if (controller.isJumping)
            return;

        Lane targetLane = null;
        Vector3 targetPosition = controller.transform.position + (_isFacingRight ? Vector3.right : Vector3.left * 3.0f);

        if (_isFacingRight)
        {
            if (_currentLane.rightNeighbor != null)
            {
                targetLane = _currentLane.rightNeighbor;
                targetPosition = _currentLane.rightNeighbor.GetJumpDestinationFrom(controller.transform.position);
            }
        }
        else
        {
            if (_currentLane.leftNeighbor != null)
            {
                targetLane = _currentLane.leftNeighbor;
                targetPosition = _currentLane.leftNeighbor.GetJumpDestinationFrom(controller.transform.position);
            }
        }

        controller.JumpTo(targetPosition, targetLane);
    }*/
}