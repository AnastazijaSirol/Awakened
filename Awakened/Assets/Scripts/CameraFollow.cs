using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Player Reference")]
    public Transform target;

    [Header("Pivot")]
    public float pivotHeight = 12f;

    [Header("Camera Positioning")]
    public float distanceBehind = 13f;
    public float heightOffset = 0f;

    [Header("Collision")]
    public LayerMask obstacleMask;
    public float collisionBuffer = 0.2f;

    [Header("Smoothing")]
    public float smoothSpeed = 0.125f;

    void LateUpdate()
    {
        if (target == null) return;

        // Pivot point above player
        Vector3 pivotPoint = target.position + Vector3.up * pivotHeight;

        // Desired camera position
        Vector3 desiredPosition = pivotPoint - target.forward * distanceBehind + Vector3.up * heightOffset;

        // Raycast for obstacle detection
        Vector3 direction = (desiredPosition - pivotPoint).normalized;
        float rayDistance = Vector3.Distance(pivotPoint, desiredPosition);

        RaycastHit hit;
        if (Physics.Raycast(pivotPoint, direction, out hit, rayDistance, obstacleMask))
        {
            // Setting the camera in front of obstacle
            float correctedDistance = hit.distance - collisionBuffer;
            correctedDistance = Mathf.Max(correctedDistance, 0.1f);
            desiredPosition = pivotPoint + direction * correctedDistance;
        }

        // Smooth camera moving
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Look at player
        transform.LookAt(pivotPoint);
    }
}