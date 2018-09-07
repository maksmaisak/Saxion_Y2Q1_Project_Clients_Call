using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public class GameplayObject : MyBehaviour
{
    protected ObjectRepresentation representation;
    private bool isRemoved;
    public float positionOnLane => currentLane.GetPositionOnLane(transform.position);

    protected Lane currentLane
    {
        get { return representation.lane; }
        set { representation.lane = value; }
    }

    protected virtual void Start()
    {
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
        Bounds bounds = GetBounds();
        float min = bounds.min.z;
        float max = bounds.max.z;

        //Debug.Log($"{min} {max} {GetKind().ToString()}");

        return representation = new ObjectRepresentation
        {
            kind = GetKind(),
            lane = GetLane(),
            gameObject = gameObject,
            positionStart = min,
            positionEnd = max
        };
    }

    public void UpdateBounds()
    {
        Bounds bounds = GetBounds();

        representation.positionStart = bounds.min.z;
        representation.positionEnd = bounds.max.z;
    }

    private Lane GetLane()
    {
        /// TEMP
        return FindObjectsOfType<Lane>()
            .OrderBy(l => Mathf.Abs(l.transform.position.x - transform.position.x))
            .FirstOrDefault();
    }

    private Bounds GetBounds()
    {
        Bounds? bounds = GetComponent<Collider>()?.bounds ?? GetComponent<Renderer>().bounds;
        Assert.IsTrue(bounds.HasValue);
        return bounds.Value;
    }

    private ObjectKind GetKind()
    {
        // TEMP
        if (CompareTag("Obstacle")) return ObjectKind.Obstacle;
        if (CompareTag("Enemy")) return ObjectKind.Enemy;
        if (CompareTag("Player")) return ObjectKind.Player;

        return ObjectKind.Platform;
    }
}