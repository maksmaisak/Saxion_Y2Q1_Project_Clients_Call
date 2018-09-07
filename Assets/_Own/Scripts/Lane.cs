using UnityEngine;
using UnityEngine.Assertions;

public class Lane : MonoBehaviour
{    
    [SerializeField] Lane _leftNeighbor;
    public Lane leftNeighbor => _leftNeighbor;
    
    [SerializeField] Lane _rightNeighbor;
    public Lane rightNeighbor => _rightNeighbor;
        
    public Vector3 GetJumpDestinationFrom(Vector3 jumpOrigin)
    {
        /// TODO Assumes all lanes are straight and axis aligned.
        jumpOrigin.x = transform.position.x;
        return jumpOrigin;
    }

    public float GetPositionOnLane(Vector3 position)
    {
        /// TEMP
        return position.z;
    }
}