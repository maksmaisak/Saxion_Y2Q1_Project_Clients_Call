using System;
using UnityEngine;
using UnityEngine.Assertions;

[Flags]
public enum ObjectKind
{
    Unassigned  = 0,
    
    Platform    = 1 << 0,
    Obstacle    = 1 << 1,
    Enemy       = 1 << 2,
    Custom      = 1 << 3,
    Player      = 1 << 4,
    
    All         = ~0
}

/// A representation of a gameplay object that is easier for detecting gameplay events.
public class ObjectRepresentation
{
    public ObjectKind kind;
    public ObjectLocation location;
    public GameObject gameObject;
}

public struct ObjectLocation
{
    /// The origin lane when moving between lanes, the current lane otherwise.
    public Lane laneA;    
    /// The target lane when moving between lanes, null otherwise.
    public Lane laneB;
    /// Assumes all lanes are parallel.
    public RangeFloat bounds;

    public bool isMovingBetweenLanes;
    public bool isBetweenLanes => laneA && laneB;

    /// Syntax sugar
    public Lane lane => laneA;
    public Lane originLane => laneA;
    public Lane targetLane => laneB;
}