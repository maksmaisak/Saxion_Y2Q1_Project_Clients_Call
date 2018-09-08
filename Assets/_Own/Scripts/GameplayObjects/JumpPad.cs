﻿using UnityEngine;
using DG.Tweening;
using UnityEngine.Assertions;

public class JumpPad : GameplayObject
{
    [SerializeField] Lane targetLane;
    [SerializeField] float playerDetectionRadius = 0.1f;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        Assert.IsNotNull(targetLane);
    }

    void FixedUpdate()
    {
        ObjectRepresentation playerRepresentation = WorldRepresentation.Instance.CheckByKind(
            ObjectKind.Player, currentLane, positionOnLane, playerDetectionRadius, areMovingObjectsAllowed: false
        );
        if (playerRepresentation == null) return;

        /// TODO Fix direct dependency.
        playerRepresentation.gameObject.GetComponent<PlayerController>().JumpTo(targetLane);
    }
}