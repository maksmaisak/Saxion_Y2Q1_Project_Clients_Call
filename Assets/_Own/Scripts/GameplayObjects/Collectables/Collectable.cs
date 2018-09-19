using UnityEngine;
using UnityEngine.Serialization;

public abstract class Collectable : GameplayObject
{
    [FormerlySerializedAs("playerDetectionRadius")] 
    [SerializeField] float playerDetectionMargin = 0.2f;
    [Tooltip("Tall collectables will be collected regardless of whether the player is jumping over it or not.")]
    [SerializeField] bool isTall;

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
            playerDetectionMargin,
            aboveLaneMatters: !isTall
        );
        if (playerRepresentation == null) return;
        if (representation.location.isBetweenLanes && !playerRepresentation.location.isBetweenLanes) return;

        OnCollected();
        enabled = false;
    }
}
