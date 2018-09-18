using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayerController))]
public class PlayerDeath : MonoBehaviour
{
    private PlayerController playerController = null;

    [SerializeField] Animator playerAnimator;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        Assert.IsNotNull(playerAnimator);
    }

    private void OnPlayerDeath()
    {
        playerController.enabled = false;

        new OnPlayerDeath(transform.position)
            .SetDeliveryType(MessageDeliveryType.Immediate)
            .PostEvent();
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
        PlayCollideAnimation();

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