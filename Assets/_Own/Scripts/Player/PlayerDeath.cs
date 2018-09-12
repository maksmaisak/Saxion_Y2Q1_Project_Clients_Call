﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayerController))]
public class PlayerDeath : MonoBehaviour
{
    private Rigidbody _rb = null;
    private PlayerController _playerController = null;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _playerController = GetComponent<PlayerController>();
    }

    private void GameOver()
    {
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
        
        _playerController.enabled = false;
        _rb.isKinematic = false;
        _rb.useGravity  = true;
        _rb.freezeRotation = true;
        _rb.detectCollisions = false;
        _rb.AddForce(transform.forward * 200.0f);
    }
}
