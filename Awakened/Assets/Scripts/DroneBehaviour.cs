using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class DroneBehaviour : MonoBehaviour
{
    [Header("Orbit settings")]
    public float orbitRadius = 2f;
    public float orbitSpeed = 45f;

    [Header("Detection settings")]
    public string playerTag = "Player";

    [Header("Shooting settings")]
    public GameObject shotPrefab;
    public float fireInterval = 1f;
    [Tooltip("Lokalni pomaci turreta")]
    public Vector3[] turretOffsets;
    public float aimHeightOffset = 1.6f;

    private Vector3 orbitCenter;
    private float currentAngle;
    private bool playerInRange = false;
    private Transform playerTransform;

    private SphereCollider detectionCollider;

    // upravljanje pucanjem
    private float fireTimer = 0f;
    private int nextTurretIndex = 0;

    void Awake()
    {
        orbitCenter = transform.position;
        detectionCollider = GetComponent<SphereCollider>();
        detectionCollider.isTrigger = true;
    }

    void Update()
    {
        // orbita
        currentAngle += orbitSpeed * Time.deltaTime;
        float rad = currentAngle * Mathf.Deg2Rad;
        Vector3 pos = orbitCenter + new Vector3(Mathf.Cos(rad), 0f, Mathf.Sin(rad)) * orbitRadius;
        transform.position = pos;

        // okreni se igraču na istoj Y razini orbitskog centra
        if (playerInRange && playerTransform != null)
        {
            Vector3 lookTarget = new Vector3(
                playerTransform.position.x,
                orbitCenter.y,
                playerTransform.position.z);
            transform.LookAt(lookTarget);

            // pucanje svakih fireInterval sekundi
            fireTimer -= Time.deltaTime;
            if (fireTimer <= 0f)
            {
                FireFromTurret(nextTurretIndex);
                nextTurretIndex = (nextTurretIndex + 1) % turretOffsets.Length;
                fireTimer = fireInterval;
            }
        }
    }

    private void FireFromTurret(int turretIdx)
    {
        if (shotPrefab == null || playerTransform == null) return;

        // izračun spawn pozicije na temelju lokalnog pomaka
        Vector3 worldOffset = transform.rotation * turretOffsets[turretIdx];
        Vector3 spawnPos = transform.position + worldOffset;

        // izračun smjera prema visini igrača
        Vector3 aimPoint = playerTransform.position + Vector3.up * aimHeightOffset;
        Vector3 dir = (aimPoint - spawnPos).normalized;

        // instanciranje metka i okretanje prema igraču
        Instantiate(shotPrefab, spawnPos, Quaternion.LookRotation(dir));
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