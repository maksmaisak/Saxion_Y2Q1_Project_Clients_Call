using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public class GameplayObject : MyBehaviour
{
    [SerializeField] ObjectKind objectKind = ObjectKind.Unassigned;
    [Tooltip("The maximum allowed difference between distances to two neighboring lanes for the object to register as being between those lanes.")]
    [SerializeField] float laneDistanceDifferenceTolerance = 1f;
    [Tooltip("The minimum height this must be above a lane to register as being above it.")]
    [SerializeField] float minHeightAboveLane = 1.49f;
    
    public ObjectRepresentation representation { get; private set; }
    private bool wasRemovedFromLevelState;

    protected float positionOnLane => representation.location.bounds.middle;
    protected Lane currentLane => representation.location.laneA;

    protected virtual void Start()
    {
        //Assert.AreNotEqual(ObjectKind.Unassigned, objectKind, $"`objectKind` was not set for {this}. Please assign in the inspector.");
        // TEMP. Use old system for determining type while transitioning away from the old system.
        if (objectKind == ObjectKind.Unassigned) objectKind = GetKindBasedOnGameObjectTag();
        
        LevelState.instance.Add(MakeRepresentation());
    }

    public void RemoveFromLevelState()
    {
        if (wasRemovedFromLevelState) return;

        LevelState.instance.Remove(representation);
        wasRemovedFromLevelState = true;
    }
    
    public void UpdateBounds()
    {
        representation.location.bounds = GetBoundsOn(representation.location.laneA);
    }

    private ObjectRepresentation MakeRepresentation()
    {
        return representation = new ObjectRepresentation
        {
            kind = objectKind,
            location = GetLocation(),
            gameObject = gameObject
        };
    }
    
    private ObjectLocation GetLocation()
    {
        // TEMP TODO A global collection of lanes.
        var lanes = FindObjectsOfType<Lane>().OrderBy(DistanceTo);

        Lane laneA = lanes.First();
        
        float distanceToA = DistanceTo(laneA);
        Lane laneB = lanes.FirstOrDefault(l => 
            l != laneA && laneA.HasNeighbor(l) && DistanceTo(l) - distanceToA <= laneDistanceDifferenceTolerance
        );
        
        return new ObjectLocation
        {
            laneA = laneA,
            bounds = GetBoundsOn(laneA),
            laneB = laneB,
            isAboveLane = IsAbove(laneA)
        }; 
    }

    protected float DistanceTo(Lane lane)
    {
        return Mathf.Abs(lane.transform.position.x - transform.position.x);
    }

    private bool IsAbove(Lane lane)
    {
        return transform.position.y - lane.transform.position.y >= minHeightAboveLane;
    }

    private RangeFloat GetBoundsOn(Lane lane)
    {
        Bounds bounds3D = GetWorldspaceBounds();

        Vector3 back  = bounds3D.center + Vector3.back    * bounds3D.extents.z;
        Vector3 front = bounds3D.center + Vector3.forward * bounds3D.extents.z;

        float min = lane.GetPositionOnLane(back );
        float max = lane.GetPositionOnLane(front);
        return new RangeFloat(min, max);
    }

    private Bounds GetWorldspaceBounds()
    {
        var col = GetComponentInChildren<Collider>();
        if (col) return col.bounds;
        
        return GetComponentInChildren<Renderer>().bounds;
    }

    private ObjectKind GetKindBasedOnGameObjectTag()
    {
        // TEMP till we completely move away from the tag-based system.
        if (CompareTag("Obstacle")) return ObjectKind.Obstacle;
        if (CompareTag("Enemy"))    return ObjectKind.Enemy;
        if (CompareTag("Player"))   return ObjectKind.Player;

        return ObjectKind.Platform;
    }
}