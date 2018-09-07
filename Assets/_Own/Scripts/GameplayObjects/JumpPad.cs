using UnityEngine;
using DG.Tweening;

public class JumpPad : MonoBehaviour
{
    [SerializeField] bool _isFacingRight = true;
    [SerializeField] Lane _currentLane = null;

    // Use this for initialization
    void Start()
    {
        Debug.Assert(_currentLane != null);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        var controller = other.GetComponent<PlayerController>();
        Debug.Assert(controller != null);

        if (controller.IsJumping())
            return;

        Lane targetLane = null;
        Vector3 targetPosition = controller.transform.position + (_isFacingRight ? Vector3.right : Vector3.left * 3.0f);

        if (_isFacingRight)
        {
            if (_currentLane.rightNeighbor != null)
            {
                targetLane = _currentLane.rightNeighbor;
                targetPosition = _currentLane.rightNeighbor.GetJumpDestinationFrom(controller.transform.position);
            }
        }
        else
        {
            if (_currentLane.leftNeighbor != null)
            {
                targetLane = _currentLane.leftNeighbor;
                targetPosition = _currentLane.leftNeighbor.GetJumpDestinationFrom(controller.transform.position);
            }
        }

        controller.JumpTo(targetPosition, targetLane);
    }
}
