using DG.Tweening;
using UnityEngine;

public enum PowerUpType
{
    Slow = 1,
    Fast,
    MaxTypes,
}

[System.Serializable]
public struct PowerUpInfo
{
    [Tooltip("Duration in seconds until the power up expires.")]
    public float duration;
    public float basepoints;
    public PowerUpType type;
}

/// TODO This needs refactoring. Checking by PowerUpType all over the place sucks a lot.
public class PowerUp : Collectable
{
    [Space]
    [SerializeField] PowerUpInfo info;
    [SerializeField] StandardCollectableAnimationSettings animations;

    protected override void OnCollected()
    {
        new OnPowerUpCollected(info).PostEvent();

        animations
            .PlayDisappearAnimation(gameObject)
            .AppendCallback(() => Destroy(gameObject));
    }
}
