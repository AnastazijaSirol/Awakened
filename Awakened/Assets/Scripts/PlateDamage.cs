using UnityEngine;

[RequireComponent(typeof(Collider))]
public class SensorPlateDamage : MonoBehaviour
{
    [Header("Reference to HealthManager")]
    public HealthManager healthManager;

    [Header("Damage Cooldown")]
    public float damageCooldown = 1f;

    private bool onCooldown = false;

    private void Awake()
    {
        Collider col = GetComponent<BoxCollider>();
        col.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        // If the player steps on sensor plate without cooldown, lose life
        if (onCooldown) return;

        if (other.CompareTag("Player"))
        {
            if (healthManager != null)
            {
                healthManager.LoseLife();
            }

            // Start cooldown coroutine
            if (damageCooldown > 0f)
                StartCoroutine(CooldownCoroutine());
        }
    }

    private System.Collections.IEnumerator CooldownCoroutine()
    {
        onCooldown = true;
        yield return new WaitForSeconds(damageCooldown);
        onCooldown = false;
    }
}