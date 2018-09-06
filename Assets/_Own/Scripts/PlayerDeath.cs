﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayerController))]
public class PlayerDeath : MonoBehaviour
{
    [SerializeField] private Vector3 _spawnPoint = Vector3.zero;
    [SerializeField] private float _minimumPositionY = -15f;

    private Rigidbody _rb = null;
    private Player _player = null;
    private PlayerController _playerController = null;

    public delegate void GameOverEvent();
    public event GameOverEvent HandleGameOver;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _player = GetComponent<Player>();
        _playerController = GetComponent<PlayerController>();
    }

    private void GameOver()
    {
        // Die
        // Play Sound
        // Play Animation
        // Display Resolution Screen
        //_rb.constraints = _rb.constraints |= RigidbodyConstraints.FreezeAll;
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        
        //ResetPlayerDefaults();

        //HandleGameOver?.Invoke();
    }

    void OnTriggerEnter(Collider other)
    {
        if (!_player.IsGodMode() && other.CompareTag("Obstacle"))
            GameOver();
    }

    private void ResetPlayerDefaults()
    {
        _playerController.enabled = true;
        _rb.useGravity = false;
        _rb.isKinematic = true;
        transform.position = _spawnPoint;
        transform.rotation = Quaternion.Euler(0, 0, 0);
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
