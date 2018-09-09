using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public class GameplayObject : MyBehaviour
{
    [SerializeField] ObjectKind objectKind = ObjectKind.Unassigned;
    
    protected ObjectRepresentation representation;
    private bool isRemoved;

    protected float positionOnLane => representation.location.bounds.middle;
    protected Lane currentLane => representation.location.laneA;

    protected virtual void Start()
    {
        //Assert.AreNotEqual(ObjectKind.Unassigned, objectKind, $"`objectKind` was not set for {this}. Please assign in the inspector.");
        // TEMP. Use old system for determining type while transitioning away from the old system.
        if (objectKind == ObjectKind.Unassigned) objectKind = GetKindBasedOnGameObjectTag();
        
        WorldRepresentation.Instance.objects.Add(MakeRepresentation());
    }

    public void RemoveFromWorldModel()
    {
        if (isRemoved) return;

        WorldRepresentation.Instance.objects.Remove(representation);
        isRemoved = true;
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

    public void UpdateBounds()
    {
        representation.location.bounds = GetBoundsOn(representation.location.laneA);
    }
    
    private ObjectLocation GetLocation()
    {
        var lanes = FindObjectsOfType<Lane>()
            .OrderBy(l => Mathf.Abs(l.transform.position.x - transform.position.x));

        Lane laneA = lanes.FirstOrDefault();
        
        // TODO Add detection of laneB

        return new ObjectLocation
        {
            laneA = laneA,
            bounds = GetBoundsOn(laneA)
        };
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
        Bounds? bounds = GetComponent<Collider>()?.bounds ?? GetComponent<Renderer>().bounds;
        Assert.IsTrue(bounds.HasValue);
        return bounds.Value;
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