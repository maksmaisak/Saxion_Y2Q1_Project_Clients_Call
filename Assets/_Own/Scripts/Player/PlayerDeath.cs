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
    [SerializeField] float delayBeforeResolutionScreen = 2f;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        playerController = GetComponent<PlayerController>();
        Assert.IsNotNull(playerAnimator);
        Debug.Assert(rigidBody != null);
    }

    private void GameOver()
    {
        new OnInsertCoinScreen().SetDeliveryType(MessageDeliveryType.Immediate).PostEvent();
    }

    public void DeathObstacle()
    {
        playerController.enabled = false;

        CollideAnimation();
        Delay(delayBeforeResolutionScreen, GameOver);
    }

    public void DeathEnemy()
    {
        playerController.enabled = false;

        CollideAnimation();
        Delay(delayBeforeResolutionScreen, GameOver);
    }

    public void DeathFall()
    {
        playerController.enabled = false;

        GameOver();
        /*rigidBody.useGravity = true;
        rigidBody.isKinematic = false;
        rigidBody.freezeRotation = true;
        rigidBody.detectCollisions = false;
        rigidBody.AddForce(transform.forward * 200.0f);*/
    }

    private void CollideAnimation()
    {
        playerAnimator.SetBool("Death", true);
    }

    private void Delay(float delay, Action action)
    {
        StartCoroutine(DelayCoroutine(delay, action));
    }

    private IEnumerator DelayCoroutine(float delay, Action action)
    {
        yield return new WaitForSeconds(delay);
        action?.Invoke();
    }
}