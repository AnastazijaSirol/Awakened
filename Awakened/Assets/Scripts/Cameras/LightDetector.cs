using UnityEngine;

public class LightDetector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Provjeri postoji li aktivan shield na playeru
            CameraShield shield = other.GetComponent<CameraShield>();
            if (shield != null && shield.IsShieldActive)
            {
                // Ako je shield aktivan, ništa se ne događa
                return;
            }

            // Inače igrač gubi život
            HealthManager health = other.GetComponent<HealthManager>();
            if (health != null)
            {
                health.LoseLife();
            }
        }
    }
}
