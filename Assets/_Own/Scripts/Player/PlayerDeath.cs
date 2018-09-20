using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

public class PlayerDeath : MyBehaviour, IEventReceiver<OnGameOver>
{
    [SerializeField] Animator playerAnimator;

    void Start()
    {
        Assert.IsNotNull(playerAnimator);
    }

    public void On(OnGameOver message) => enabled = false;
    
    public void DeathObstacle()
    {
        if (!enabled) return;
        
        OnPlayerDeath();
        PlayCollideAnimation();
    }

    public void DeathEnemy()
    {
        if (!enabled) return;
        
        OnPlayerDeath();
        PlayCollideAnimation();
    }

    public void DeathFall()
    {
        if (!enabled) return;
        
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
    
    private void OnPlayerDeath()
    {
        new OnPlayerDeath(transform.position, SceneManager.GetActiveScene().name)
            .SetDeliveryType(MessageDeliveryType.Immediate)
            .PostEvent();
    }

}