using UnityEngine;

public class Lane : MonoBehaviour
{
    private const float JumpLength = 3f;
    
    [SerializeField] Lane _leftNeighbor;
    public Lane leftNeighbor => _leftNeighbor;
    
    [SerializeField] Lane _rightNeighbor;
    public Lane rightNeighbor => _rightNeighbor;
    
    public Vector3 GetJumpDestinationFrom(Vector3 jumpOrigin)
    {
        /// TODO Assumes all lanes are straight and axis aligned.
        jumpOrigin.x = transform.position.x;
        jumpOrigin.z += JumpLength;
        return jumpOrigin;
    }
}