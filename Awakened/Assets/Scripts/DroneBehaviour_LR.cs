using UnityEngine;

public class DroneBehaviour_LR : MonoBehaviour
{
    [Tooltip("Maximum distance to sway along the initial local X axis")]
    public float moveAmplitude = 20f;
    [Tooltip("Speed of the sway motion")]
    public float moveSpeed = 1f;

    [Header("Detection settings")]
    [Tooltip("Tag used to identify the player")]
    public string playerTag = "Player";
    [Tooltip("Radius around the drone center to detect the player")]
    public float detectionRadius = 6f;

    [Header("Shooting settings")]
    [Tooltip("Prefab of the projectile to spawn")]
    public GameObject shotPrefab;
    [Tooltip("Time in seconds between shots")]
    public float fireInterval = 0.2f;
    [Tooltip("Local offsets for each turret spawn point")]
    public Vector3[] turretOffsets;
    [Tooltip("Vertical offset to aim at the player's body")]
    public float aimHeightOffset = 14f;

    // cached references & state
    private Vector3 startPos;
    private Vector3 swayAxis;
    private float moveTimer;
    private Transform playerTransform;
    private float fireTimer;
    private int nextTurretIndex;

    void Awake()
    {
        // cache the starting center and the initial local X axis
        startPos = transform.position;
        swayAxis = transform.right;
    }

    void Update()
    {
        // Detect player around the fixed center
        if (playerTransform == null)
        {
            var hits = Physics.OverlapSphere(startPos, detectionRadius);
            foreach (var c in hits)
            {
                if (c.CompareTag(playerTag))
                {
                    playerTransform = c.transform;
                    break;
                }
            }
        }
        else
        {
            if ((playerTransform.position - startPos).sqrMagnitude > detectionRadius * detectionRadius)
                playerTransform = null;
        }

        // Sway left–right along the fixed swayAxis
        moveTimer += Time.deltaTime * moveSpeed;
        float offset = Mathf.Sin(moveTimer) * moveAmplitude;
        transform.position = startPos + swayAxis * offset;

        // If player is in range, face & shoot
        if (playerTransform != null)
        {
            // rotate around Y only, keep original height
            var lookTarget = new Vector3(
                playerTransform.position.x,
                startPos.y,
                playerTransform.position.z
            );
            transform.LookAt(lookTarget);

            // fire logic
            fireTimer -= Time.deltaTime;
            if (fireTimer <= 0f)
            {
                FireFromTurret(nextTurretIndex);
                nextTurretIndex = (nextTurretIndex + 1) % turretOffsets.Length;
                fireTimer = fireInterval;
            }
        }
    }

    private void FireFromTurret(int idx)
    {
        if (shotPrefab == null || playerTransform == null) return;

        // spawn at the correctly rotated turret offset
        Vector3 spawnPos = transform.TransformPoint(turretOffsets[idx]);

        // aim at the player's body
        Vector3 aimPoint = playerTransform.position + Vector3.up * aimHeightOffset;
        Vector3 dir = (aimPoint - spawnPos).normalized;

        Instantiate(shotPrefab, spawnPos, Quaternion.LookRotation(dir));
    }
}