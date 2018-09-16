using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(PlayerController))]
public class PlayerDeath : MonoBehaviour
{
    private Rigidbody rigidBody = null;
    private PlayerController playerController = null;

    [SerializeField] Animator playerAnimator;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        playerController = GetComponent<PlayerController>();
        Assert.IsNotNull(playerAnimator);
        Debug.Assert(rigidBody != null);
    }

    private void OnPlayerDeath()
    {
        playerController.enabled = false;
        new OnPlayerDeath().SetDeliveryType(MessageDeliveryType.Immediate).PostEvent();
    }

    public void DeathObstacle()
    {
        OnPlayerDeath();
        PlayCollideAnimation();
    }

    public void DeathEnemy()
    {
        OnPlayerDeath();
        PlayCollideAnimation();
    }

    public void DeathFall()
    {
        OnPlayerDeath();

        /*rigidBody.useGravity = true;
        rigidBody.isKinematic = false;
        rigidBody.freezeRotation = true;
        rigidBody.detectCollisions = false;
        rigidBody.AddForce(transform.forward * 200.0f);*/
    }

    private void PlayCollideAnimation()
    {
        playerAnimator.SetBool("Death", true);
    }
}