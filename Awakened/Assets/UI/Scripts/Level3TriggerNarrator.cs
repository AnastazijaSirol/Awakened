using UnityEngine;

public class Level3TriggerNarrator : MonoBehaviour
{
    public Level3NarratorSubtitles narrator;
    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!hasTriggered && other.CompareTag("Player"))
        {
            hasTriggered = true;
            narrator?.StartNarration();
        }
    }
}
