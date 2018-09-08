using UnityEngine;

public abstract class Collectable : GameplayObject
{
    [SerializeField] protected float playerDetectionRadius = 0.2f;

    public abstract void OnCollected();

    public void CheckPlayer()
    {
        if (WorldRepresentation.Instance.CheckByKind(
            ObjectKind.Player, currentLane, positionOnLane, playerDetectionRadius) != null)
        {
            enabled = false;
            OnCollected();
        }
    }

    protected virtual void FixedUpdate()
    {
        CheckPlayer();
    }
}
