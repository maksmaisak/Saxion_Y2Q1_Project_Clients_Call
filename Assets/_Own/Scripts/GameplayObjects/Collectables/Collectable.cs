using UnityEngine;

public abstract class Collectable : GameplayObject
{
    [SerializeField] protected float playerDetectionRadius = 0.2f;

    protected virtual void FixedUpdate()
    {
        CheckPlayer();
    }

    protected abstract void OnCollected();

    private void CheckPlayer()
    {
        var playerRepresentation = WorldRepresentation.Instance.CheckIntersect(
            representation, 
            ObjectKind.Player, 
            playerDetectionRadius
        );
        if (playerRepresentation == null) return;
        if (representation.location.isBetweenLanes && !playerRepresentation.location.isBetweenLanes) return;

        OnCollected();
        enabled = false;
    }
}
