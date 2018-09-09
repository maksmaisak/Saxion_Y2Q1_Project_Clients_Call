using UnityEngine;
using System.Collections.Generic;

public class WorldRepresentation : Singleton<WorldRepresentation>
{  
    public List<ObjectRepresentation> objects = new List<ObjectRepresentation>();

    public ObjectRepresentation CheckByKind(ObjectKind kind, Lane lane, float position, float tolerance = 0f, bool areMovingObjectsAllowed = true)
    {
        foreach (ObjectRepresentation record in objects)
        {
            if (record.kind != kind) continue;

            bool isMoving = record.destinationLane != null;
            if (!areMovingObjectsAllowed && isMoving) continue;

            if (record.lane != lane)
            {
                if (!areMovingObjectsAllowed) continue;
                if (record.destinationLane != lane) continue;
            }

            if (record.IsCloserThan(tolerance, position)) return record;
        }

        return null;
    }

    public ObjectRepresentation CheckEnemy(Lane lane, float position)
    {
        return CheckByKind(ObjectKind.Enemy, lane, position);
    }
}