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

    [Header("Shake Settings")]
    [Tooltip("Duration in seconds")]
    public float shakeDuration = 0.5f;
    [Tooltip("Magnitude")]
    public float shakeMagnitude = 0.5f;

    // Internal tracker of shaking time
    private float shakeTimeRemaining = 0f;

    private void OnEnable()
    {
        // Subscribe on event when player loses a life
        HealthManager.OnLifeLost += TriggerShake;
    }

    private void OnDisable()
    {
        HealthManager.OnLifeLost -= TriggerShake;
    }

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

        // Smooth camera moving to desiredPosition
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // If shaking is active, add random offset
        if (shakeTimeRemaining > 0f)
        {
            Vector3 randomOffset = Random.insideUnitSphere * shakeMagnitude;
            smoothedPosition += randomOffset;
            shakeTimeRemaining -= Time.deltaTime;
        }

        // Set final camera position
        transform.position = smoothedPosition;

        // Look at player
        transform.LookAt(pivotPoint);
    }

    // Method for triggering camera shake
    private void TriggerShake()
    {
        shakeTimeRemaining = shakeDuration;
    }
}