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
        JumpRight,
        JumpForward
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

            Lane targetLane = GetTargetJumpLane();
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
        Vector2 input = new Vector2(
            Input.GetAxis("Horizontal"),
            Input.GetAxis("Vertical")
        );
        
        if      (input.x >  0.01f) currentInput = InputKind.JumpRight;
        else if (input.x < -0.01f) currentInput = InputKind.JumpLeft;
        else if (input.y >  0.01f) currentInput = InputKind.JumpForward;
        else currentInput = InputKind.None;
    }

    private void CheckFall()
    {
        var ray = new Ray(transform.position, Vector3.down);
        if (!Physics.SphereCast(ray, 0.4f, 20f))
        {
            Fall();
        }
    }

    private Lane GetTargetJumpLane()
    {
        switch (currentInput)
        {
            case InputKind.JumpLeft: return currentLane.leftNeighbor;
            case InputKind.JumpRight: return currentLane.rightNeighbor;
            case InputKind.JumpForward: return currentLane;
        }

        return null;
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