using System.Runtime.Remoting.Messaging;
using UnityEngine;

// Detects if this GameplayObject is close to the edge of a platform and sets the appropriate
// flag in LevelState to prevent things from moving.
[RequireComponent(typeof(GameplayObject))]
public class PlayerCloseToPlatformEdgeDetector : MyBehaviour
{
    private GameplayObject obj;
    private ObjectRepresentation representation => obj.representation;
    private LevelState level;
    
    [Tooltip("Platforms can't move if player is closer than this to the edge of it.")]
    [SerializeField] float minDistanceFromPlayerToEdgeToMove = 1f;
    
    void Start()
    {
        obj = GetComponent<GameplayObject>();
        level = LevelState.instance;
    }

    void Update()
    {
        level.canPlatformsMove = IsPlayerFarEnoughFromEdges();
    }

    private bool IsPlayerFarEnoughFromEdges()
    {
        var platform = level.CheckIntersect(
            representation, 
            ObjectKind.Platform, 
            margin: minDistanceFromPlayerToEdgeToMove, 
            aboveLaneMatters: false
        );

        // If too far outside.
        if (platform == null) return true;

        // If too far inside.
        return platform.location
            .bounds
            .Inflated(0f, -minDistanceFromPlayerToEdgeToMove)
            .Intersects(representation.location.bounds);
    }
}
