using UnityEngine;
using UnityEngine.Serialization;

public class SmoothFollow : MonoBehaviour
{
    // The target we are following
    [SerializeField] Transform target;
    // The distance in the x-z plane to the target
    [SerializeField] float desiredDistanceXZ = 4f;
    // the height we want the camera to be above the target
    [SerializeField] float desiredDistanceY = 2f;

    [Header("Damping (0 is instant snapping, 1 is no motion at all)")]
    [SerializeField] Vector3 positionDamping = Vector3.one * 0.01f;
    [SerializeField] float rotationDamping;

    void LateUpdate()
    {
        if (!target) return;

        // Calculate the current rotation angles
        float desiredRotationAngle = target.eulerAngles.y;
        float currentRotationAngle = transform.eulerAngles.y;

        // Damp the rotation around the y-axis
        currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, desiredRotationAngle, rotationDamping * Time.deltaTime);

        // Convert the angle into a rotation
        var currentRotation = Quaternion.Euler(0f, currentRotationAngle, 0f);
        
        // Set the position of the camera on the x-z plane to:
        // distance meters behind the target
        Vector3 desiredPosition =
            target.position +
            currentRotation * Vector3.back * desiredDistanceXZ +
            Vector3.up * desiredDistanceY;

        Vector3 delta = desiredPosition - transform.position;
        delta.x *= Mathf.Pow(positionDamping.x, Time.unscaledDeltaTime);
        delta.y *= Mathf.Pow(positionDamping.y, Time.unscaledDeltaTime);
        delta.z *= Mathf.Pow(positionDamping.z, Time.unscaledDeltaTime);
        transform.position = desiredPosition - delta;
    }

    /// Provides the t-value for a Lerp, which would make the Lerp, when applied once per frame,
    /// equivalent to multiplying the distance between the targets by dampingCoef.
    private float LerpDamp(float dampingCoef)
    {
        return 1f - Mathf.Pow(dampingCoef, Time.deltaTime);
    }
}