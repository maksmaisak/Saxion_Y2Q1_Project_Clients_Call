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
    public float power;
    public PowerUpType type;
}

public class PowerUp : MonoBehaviour {

    [SerializeField] PowerUpInfo info;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        Player player = other.GetComponent<Player>();
        if (!player)
            return;

        player.CollectPowerUp(info);

        Destroy(this.gameObject);
    }
}
