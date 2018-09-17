using UnityEngine;
using UnityEngine.Serialization;

public abstract class Collectable : GameplayObject
{
    [FormerlySerializedAs("playerDetectionRadius")] 
    [SerializeField] 
    protected float playerDetectionMargin = 0.2f;

    protected virtual void FixedUpdate()
    {
        CheckPlayer();
    }

    protected abstract void OnCollected();

    private void CheckPlayer()
    {
        var playerRepresentation = LevelState.instance.CheckIntersect(
            representation, 
            ObjectKind.Player, 
            playerDetectionMargin
        );
        if (playerRepresentation == null) return;
        if (representation.location.isBetweenLanes && !playerRepresentation.location.isBetweenLanes) return;

        OnCollected();
        enabled = false;
    }
}
