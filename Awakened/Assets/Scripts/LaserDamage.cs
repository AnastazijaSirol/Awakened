using UnityEngine;

[RequireComponent(typeof(Collider))]
public class LaserDamage : MonoBehaviour
{
    public float damageAmount = 1f;

    void Awake()
    {
        var col = GetComponent<Collider>();
        if (!col.isTrigger)
        {
            col.isTrigger = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (PlayerStats.Instance != null)
            {
                PlayerStats.Instance.TakeDamage(damageAmount);
            }
        }
    }
}