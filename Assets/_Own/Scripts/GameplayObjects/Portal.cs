using System;
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
    
    void FixedUpdate()
    {
        var playerRepresentation = WorldRepresentation.instance.CheckIntersect(
            representation, 
            ObjectKind.Player, 
            playerDetectionMargin,
            aboveLaneMatters: false
        );
        if (playerRepresentation == null) return;
        if (representation.location.isAboveLane && !playerRepresentation.location.isAboveLane) return;
      
        LevelManager.instance.LoadLevel(nextLevelName, pauseTimeWhileLoading: false);
        new OnPortalEntered(kind).PostEvent();
        
        /*switch (kind)
        {
            case Kind.LevelExit:
                new OnLevelExit().PostEvent();
                break;
            case Kind.BonusLevelEntry:
                new OnBonusLevelEntry().PostEvent();
                break;
            case Kind.BonusLevelExit:
                new OnBonusLevelExit().PostEvent();
                break;
            default:
                throw new ArgumentOutOfRangeException($"Invalid portal kind: {kind}");
        }*/

        enabled = false;
    }
}
