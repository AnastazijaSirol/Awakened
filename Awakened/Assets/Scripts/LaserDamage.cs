using UnityEngine;

[RequireComponent(typeof(Collider))]
public class LaserDamage : MonoBehaviour
{
    public HealthManager healthManager;

    public float damageCooldown = 1f;

    private bool onCooldown = false;

    private void Awake()
    {
        Collider col = GetComponent<Collider>();
        col.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (onCooldown) return;

        if (other.CompareTag("Player"))
        {
            if (healthManager != null)
            {
                healthManager.LoseLife();
            }

            // Start cooldown so player doesn't lose more lives while in laser zone
            if (damageCooldown > 0f)
                StartCoroutine(DamageCooldownCoroutine());
        }
    }

    private System.Collections.IEnumerator DamageCooldownCoroutine()
    {
        onCooldown = true;
        yield return new WaitForSeconds(damageCooldown);
        onCooldown = false;
    }
}