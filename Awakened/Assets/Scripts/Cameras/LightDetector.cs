using UnityEngine;

public class LightDetector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HealthManager health = other.GetComponent<HealthManager>();
            if (health != null)
            {
                health.LoseLife();
            }
        }
    }
}
