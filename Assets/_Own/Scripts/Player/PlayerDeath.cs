using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayerController))]
public class PlayerDeath : MonoBehaviour
{
    private Rigidbody rigidBody = null;
    private PlayerController playerController = null;

    [SerializeField] Animator playerAnimator;
    [SerializeField] float delayBeforeResolutionScreen = 2f;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        playerController = GetComponent<PlayerController>();
        Assert.IsNotNull(playerAnimator);
    }

    private void GameOver()
    {
        new OnGameOver().PostEvent();
    }

    public void DeathObstacle()
    {
        playerController.enabled = false;

        CollideAnimation();
        this.Delay(delayBeforeResolutionScreen, GameOver);
    }

    public void DeathEnemy()
    {
        playerController.enabled = false;

        CollideAnimation();
        this.Delay(delayBeforeResolutionScreen, GameOver);
    }

    public void DeathFall()
    {
        playerController.enabled = false;

        GameOver();

        playerController.enabled = false;
        rigidBody.isKinematic = false;
        rigidBody.useGravity = true;
        rigidBody.freezeRotation = true;
        rigidBody.detectCollisions = false;
        rigidBody.AddForce(transform.forward * 200.0f);
    }

    private void CollideAnimation()
    {
        playerAnimator.SetBool("Death", true);
    }
}