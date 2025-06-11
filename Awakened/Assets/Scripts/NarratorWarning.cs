using UnityEngine;

public class NarratorEarlyWarning : MonoBehaviour
{
    public AudioSource narratorAudio;
    public NarratorSubtitlesMAIN subtitleSystem; // referenca na titlove
    public bool playOnlyOnce = true;

    private bool hasPlayed = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && (!hasPlayed || !playOnlyOnce))
        {
            // Pokreni audio + titlove zajedno
            if (subtitleSystem != null)
            {
                subtitleSystem.StartSubtitles();
            }
            else if (narratorAudio != null)
            {
                narratorAudio.Play();
            }

            hasPlayed = true;
        }
    }
}