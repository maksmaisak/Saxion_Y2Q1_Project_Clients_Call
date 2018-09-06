using UnityEngine;
using System.Collections.Generic;

public class WorldRepresentation : Singleton<WorldRepresentation>
{  
    public List<ObjectRepresentation> objects = new List<ObjectRepresentation>();
}

public enum ObjectKind
{
    Platform,
    Obstacle,
    Enemy
}

public class ObjectRepresentation
{
    public ObjectKind kind;
    public Lane lane;
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