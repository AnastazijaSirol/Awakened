using UnityEngine;

public class Level3TriggerNarrator : MonoBehaviour
{
    public Level3NarratorSubtitles narrator;
    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasTriggered)
        {
            Debug.Log("Trigger already used, ignoring extra enter.");
            return;
        }

        if (other.CompareTag("Player"))
        {
            hasTriggered = true;
            Debug.Log($"Level 3 narrator triggered by: {other.name} on {gameObject.name}");
            narrator?.StartNarration();
        }
    }

}