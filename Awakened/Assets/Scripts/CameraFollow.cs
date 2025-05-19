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

    [Header("Smoothing")]
    public float smoothSpeed = 0.125f;

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 pivotPoint = target.position + Vector3.up * pivotHeight;

        Vector3 desiredPosition = pivotPoint - target.forward * distanceBehind + Vector3.up * heightOffset;

        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        transform.LookAt(pivotPoint);
    }
}