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
        PlayFallAnimation();
    }

    private void PlayCollideAnimation()
    {
        playerAnimator.SetTrigger("Death_Obstacle");
    }

    private void PlayFallAnimation()
    {
        playerAnimator.SetTrigger("Death_Fall");
    }
}