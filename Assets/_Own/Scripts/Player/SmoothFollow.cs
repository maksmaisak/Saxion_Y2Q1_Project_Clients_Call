using UnityEngine;

public class SmoothFollow : MonoBehaviour
{
    // The target we are following
    [SerializeField]
    private Transform target;
    // The distance in the x-z plane to the target
    [SerializeField]
    private float desiredDistance = 10.0f;
    // the height we want the camera to be above the target
    [SerializeField]
    private float height = 5.0f;

    [SerializeField]
    private float rotationDamping;
    [SerializeField]
    private float heightDamping;
    [SerializeField]
    private float horizontalDamping;
    [SerializeField]
    private float distanceDamping;

    // Use this for initialization
    void Start() { }

    // Update is called once per frame
    void LateUpdate()
    {
        // Early out if we don't have a target
        if (!target)
            return;

        // Calculate the current rotation angles
        float wantedRotationAngle = target.eulerAngles.y;
        float wantedHeight = target.position.y + height;

        float currentRotationAngle = transform.eulerAngles.y;
        float currentHeight = transform.position.y;
        float currentPositionX = transform.position.x;

        // Damp the rotation around the y-axis
        currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);

        // Damp the height
        currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);

        // Damp the x position
        currentPositionX = Mathf.Lerp(currentPositionX, target.position.x, horizontalDamping * Time.deltaTime);

        // Convert the angle into a rotation
        var currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

        // Set the position of the camera on the x-z plane to:
        // distance meters behind the target
        Vector3 targetPosition = target.position - currentRotation * transform.forward * desiredDistance;

        if ((targetPosition - transform.position).magnitude <= Mathf.Epsilon)
            transform.position = targetPosition;
        else
            transform.position = Vector3.Lerp(transform.position, targetPosition, 1f - Mathf.Pow(distanceDamping, Time.deltaTime));

        // Set the height of the camera
        transform.position = new Vector3(currentPositionX, currentHeight, transform.position.z);
    }
}