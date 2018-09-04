using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    [SerializeField]
    private Vector3 _spawnPoint = Vector3.zero;
    [SerializeField]
    private float _minimumPositionY = -15f;

    private Rigidbody _rb = null;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        StartCoroutine(CheckYBoundary());
    }

    void GameOver()
    {
        // Die
        // Play Sound
        // Play Animation
        // Display Resolution Screen
        ActivateRigidBodyConstraints(false);
        transform.position = _spawnPoint;
        ActivateRigidBodyConstraints(true);
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.CompareTag("Obstacle"))
            GameOver();
    }

    void ActivateRigidBodyConstraints(bool enabled)
    {
        Debug.Assert(_rb != null);

        _rb.detectCollisions = enabled;
        _rb.useGravity = enabled;
        _rb.freezeRotation = !enabled;
    }

    IEnumerator CheckYBoundary()
    {
        while(this.enabled)
        {
            if(transform.position.y < _minimumPositionY)
                GameOver();

            yield return new WaitForSeconds(0.3f);
        }
    }
}
