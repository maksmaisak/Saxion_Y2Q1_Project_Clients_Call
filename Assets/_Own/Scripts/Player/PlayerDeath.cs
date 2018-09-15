using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class PlayerDeath : MonoBehaviour
{
    private Rigidbody rigidBody = null;
    private PlayerController playerController = null;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        playerController = GetComponent<PlayerController>();

        Debug.Assert(rigidBody != null);
    }

    private void GameOver()
    {
        playerController.enabled = false;
        new OnInsertCoinScreen().SetDeliveryType(MessageDeliveryType.Immediate).PostEvent();
    }

    public void DeathObstacle()
    {
        GameOver();
    }

    public void DeathEnemy()
    {
        GameOver();
    }

    public void DeathFall()
    {
        GameOver();
    }
}
