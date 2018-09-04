using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Assertions;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float speedForward = 0.5f;
    [SerializeField] float jumpDistance = 4f;
    [SerializeField] float jumpDuration = 0.2f;
    [Space] 
    [SerializeField] new Rigidbody rigidbody;
    [SerializeField] Lane currentLane;

    private Lane _startingLane;

    enum InputKind
    {
        None,
        JumpLeft,
        JumpRight
    }

    private InputKind currentInput;
    private bool isJumping;

    void Start()
    {
        Assert.IsNotNull(rigidbody);
        Assert.IsNotNull(currentLane);

        _startingLane = currentLane;

        PlayerDeath deathScript = GetComponent<PlayerDeath>();
        if (deathScript != null)
            deathScript.HandleGameOver += HandleGameOver;
    }
    
    void FixedUpdate()
    {
        transform.position += Vector3.forward * speedForward * Time.fixedDeltaTime;

        if (!isJumping)
        {
            if (currentInput == InputKind.None)
            {
                CheckFall();
                return;
            }
      
            Lane targetLane = currentInput == InputKind.JumpRight ? currentLane.rightNeighbor : currentLane.leftNeighbor;
            if (targetLane == null) return;

            Vector3 targetPosition = targetLane.GetJumpDestinationFrom(transform.position);
            transform
                .DOJump(targetPosition, 1f, 1, jumpDuration)
                .OnComplete(() => isJumping = false);

            isJumping = true;
            currentLane = targetLane;
        }
    }

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        if      (horizontalInput >  0.01f) currentInput = InputKind.JumpRight;
        else if (horizontalInput < -0.01f) currentInput = InputKind.JumpLeft;
        else currentInput = InputKind.None;
    }

    private void CheckFall()
    {
        if (!Physics.Raycast(transform.position, Vector3.down, 20f))
        {
            Fall();
        }
    }

    private void Fall()
    {
        enabled = false;
        rigidbody.isKinematic = false;
        rigidbody.useGravity  = true;
        rigidbody.AddForce(transform.forward * 200.0f);
    }

    private void HandleGameOver()
    {
        currentLane = _startingLane;
    }
}