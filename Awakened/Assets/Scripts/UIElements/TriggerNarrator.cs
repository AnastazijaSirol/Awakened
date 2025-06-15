using UnityEngine;

public class TriggerNarrator : MonoBehaviour
{
    public narratorSubtitles2 narrator;
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
