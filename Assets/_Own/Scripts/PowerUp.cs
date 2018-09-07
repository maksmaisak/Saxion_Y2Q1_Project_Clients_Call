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

public class PowerUp : Collectable
{
    [SerializeField] PowerUpInfo info;

    public override void OnCollected()
    {
        new PowerUpCollected(info).PostEvent();
        Destroy(this);
    }
}
