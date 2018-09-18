using System;
using DG.Tweening;
using UnityEngine;

public class Portal : GameplayObject
{
    public enum Kind
    {
        LevelExit,
        BonusLevelEntry,
        BonusLevelExit
    }
    
    [Space]
    [SerializeField] float playerDetectionMargin = 0.2f;
    [SerializeField] Kind kind;
    [SerializeField] string nextLevelName;
    [Space] 
    [SerializeField] Transform inside;
    [SerializeField] float rotationDegreesPerSecond = 180f;

    protected override void Start()
    {
        base.Start();
        RotateInside();
    }

    void FixedUpdate()
    {
        var playerRepresentation = LevelState.instance.CheckIntersect(
            representation, 
            ObjectKind.Player, 
            playerDetectionMargin,
            aboveLaneMatters: false
        );
        if (playerRepresentation == null) return;
        if (representation.location.isAboveLane && !playerRepresentation.location.isAboveLane) return;
      
        LevelManager.instance.LoadLevel(nextLevelName, pauseTimeWhileLoading: false);
        new OnPortalEntered(kind).PostEvent();

        enabled = false;
    }

    private void RotateInside()
    {
        if (!inside) return;

        inside
            .DOLocalRotate(Vector3.forward * rotationDegreesPerSecond, 1f, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Incremental);
    }
}
