using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

public class PlayerDeath : MonoBehaviour
{
    [SerializeField] Animator playerAnimator;

    void Start()
    {
        Assert.IsNotNull(playerAnimator);
    }

    private void OnPlayerDeath()
    {
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