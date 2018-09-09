using UnityEngine;
using UnityEngine.Assertions;

public enum ObjectKind
{
    Unassigned,
    Platform,
    Obstacle,
    Enemy,
    Collectable,
    Player
}

/// A representation of a gameplay object that is easier for detecting gameplay events.
public class ObjectRepresentation
{
    public ObjectKind kind;
    public Lane lane;
    /// Null if the object is not moving. If the object is mid-movement between lanes, `lane` is the lane of origin, and `destinationLane` is the lane of destination.
    public Lane destinationLane;
    public GameObject gameObject;
        
    public float positionStart;
    public float positionEnd;

    public bool IsInside(float point)
    {
        return positionStart <= point && positionEnd >= point;
    }

    public bool IsCloserThan(float distance, float point)
    {
        return
            positionStart - distance <= point &&
            positionEnd   + distance >= point;
    }
}

public struct ObjectLocation
{
    public Lane laneA;
    public RangeFloat boundsLaneA;
    
    public Lane laneB;
    public RangeFloat boundsLaneB;

    public bool isMoving;
    public bool isBetweenLanes => laneA && laneB;

    #region Steady on one lane
    public RangeFloat bounds
    {
        get
        {
            Assert.IsFalse(isBetweenLanes, "Can't get `bounds` on an object that's between lanes. Use boundsLaneA or boundsLaneB instead.");
            return boundsLaneA;
        }
    }

    public Lane lane
    {
        get
        {
            Assert.IsFalse(isBetweenLanes, "Can't get `lane` on an object that's between lanes. Use lanaA or laneB instead.");
            return laneA;
        }
    }
    #endregion

    #region Moving between lanes
    
    public Lane originLane
    {
        get
        {
            Assert.IsTrue(isBetweenLanes && isMoving, "Can't get `originLane` that is not moving between lanes.");
            return laneA;
        }
    }
    
    public Lane destinationLane
    {
        get
        {
            Assert.IsTrue(isBetweenLanes && isMoving, "Can't get `destinationLane` that is not moving between lanes.");
            return laneB;
        }
    }
    
    #endregion
}