using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class DroneBehaviour : MonoBehaviour
{
    [Header("Orbit settings")]
    [Tooltip("Radius of the circular orbit around the initial position")]    
    public float orbitRadius = 2f;
    [Tooltip("Speed of orbit in degrees per second")]    
    public float orbitSpeed = 45f;

    [Header("Detection settings")]
    [Tooltip("Tag of the player to detect")]    
    public string playerTag = "Player";

    private Vector3 orbitCenter;
    private float currentAngle;
    private bool playerInRange = false;
    private Transform playerTransform;

    private SphereCollider detectionCollider;

    void Awake()
    {
        // Cache initial center and ensure trigger collider
        orbitCenter = transform.position;
        detectionCollider = GetComponent<SphereCollider>();
        detectionCollider.isTrigger = true;
    }

    void Update()
    {
        // Update orbit position
        currentAngle += orbitSpeed * Time.deltaTime;
        float radians = currentAngle * Mathf.Deg2Rad;
        float x = orbitCenter.x + Mathf.Cos(radians) * orbitRadius;
        float z = orbitCenter.z + Mathf.Sin(radians) * orbitRadius;
        transform.position = new Vector3(x, orbitCenter.y, z);

        // If player detected, rotate to face them
        if (playerInRange && playerTransform != null)
        {
            Vector3 targetPos = new Vector3(
                playerTransform.position.x, 
                orbitCenter.y, 
                playerTransform.position.z);
            transform.LookAt(targetPos);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            playerInRange = true;
            playerTransform = other.transform;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            playerInRange = false;
            playerTransform = null;
        }
    }
}