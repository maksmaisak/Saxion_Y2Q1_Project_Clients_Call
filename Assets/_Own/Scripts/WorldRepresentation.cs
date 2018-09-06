using UnityEngine;
using System.Collections.Generic;

public class WorldRepresentation : Singleton<WorldRepresentation>
{  
    public List<ObjectRepresentation> objects = new List<ObjectRepresentation>();

    public ObjectRepresentation CheckByKind(ObjectKind kind, Lane lane, float position, float tolerance = 0f)
    {
        foreach (ObjectRepresentation record in objects)
        {
            if (record.kind != kind) continue;
            if (record.lane != lane) continue;

            if (record.IsCloserThan(tolerance, position)) return record;
        }

        return null;
    }

    public ObjectRepresentation CheckEnemy(Lane lane, float position)
    {
        return CheckByKind(ObjectKind.Enemy, lane, position);
    }
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