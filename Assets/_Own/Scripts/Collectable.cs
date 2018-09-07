using UnityEngine;

public abstract class Collectable : GameplayObject
{
    [SerializeField] protected float searchRadius = 2.0f;

    public abstract void OnCollected();

    public void CheckPlayer()
    {
        if (WorldRepresentation.Instance.CheckByKind(
            ObjectKind.Player, currentLane, positionOnLane, searchRadius) != null)
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
