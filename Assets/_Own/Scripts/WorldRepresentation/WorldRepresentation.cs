using System;
using UnityEngine;
using System.Collections.Generic;

public class WorldRepresentation : Singleton<WorldRepresentation>
{  
    public List<ObjectRepresentation> objects = new List<ObjectRepresentation>();

    public ObjectRepresentation CheckByKind(ObjectKind kind, Lane lane, float position, float margin = 0f, bool areMovingObjectsAllowed = true)
    {
        // TODO Broadphase optimization here
        
        foreach (ObjectRepresentation record in objects)
        {
            if (record.kind != kind) continue;

            if (!areMovingObjectsAllowed && record.location.isMovingBetweenLanes) continue;

            if (record.location.laneA != lane)
            {
                if (!areMovingObjectsAllowed) continue;
                if (record.location.laneB != lane) continue;
            }

            if (record.location.bounds.Contains(position, margin)) return record;
        }

        return null;
    }

    public ObjectRepresentation CheckIntersect(ObjectRepresentation obj, ObjectKind allowedKinds, float margin = 0f)
    {
        // TODO Broadphase optimization here
        
        foreach (ObjectRepresentation other in objects)
        {
            if ((other.kind & allowedKinds) == 0) continue;
            if (obj.location.isAboveLane != other.location.isAboveLane) continue;
            if (!CommonLanes(obj.location, other.location)) continue;
            
            if (obj.location.bounds.Intersects(other.location.bounds, margin)) return other;
        }

        return null;
    }

    private bool CommonLanes(ObjectLocation a, ObjectLocation b)
    {
        if (a.isBetweenLanes && b.isBetweenLanes)
        {
            // Only if they are between the same two lanes
            if (a.laneA == b.laneA && a.laneB == b.laneB) return true;
            if (a.laneA == b.laneB && a.laneB == b.laneA) return true;
            return false;
        }

        if (a.laneA == b.laneA) return true;
        if (a.isBetweenLanes && a.laneB == b.laneA) return true;
        if (b.isBetweenLanes && b.laneB == a.laneA) return true;

        return false;
    }
}