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
    
    enum InputKind
    {
        None,
        JumpLeft,
        JumpRight
    }

    private InputKind currentInput;
    private bool isJumping;

    [SerializeField] Lane currentLane;

    void Start()
    {
        Assert.IsNotNull(currentLane);
    }
    
    void FixedUpdate()
    {
        transform.position += Vector3.forward * speedForward * Time.fixedDeltaTime;

        if (!isJumping && currentInput != InputKind.None)
        {            
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
}