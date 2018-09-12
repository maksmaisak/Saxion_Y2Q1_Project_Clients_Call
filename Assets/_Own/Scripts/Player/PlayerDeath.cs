﻿﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayerController))]
public class PlayerDeath : MonoBehaviour
{
    private Rigidbody rigidBody = null;
    private PlayerController playerController = null;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        playerController = GetComponent<PlayerController>();
    }

    private void GameOver()
    {
        playerController.enabled = false;
        new OnGameOver().PostEvent();
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

        rigidBody.isKinematic = false;
        rigidBody.useGravity  = true;
        rigidBody.freezeRotation = true;
        rigidBody.detectCollisions = false;
        rigidBody.AddForce(transform.forward * 200.0f);
    }
}
