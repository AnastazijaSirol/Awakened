using UnityEngine;

public class PlateTrigger : MonoBehaviour
{
    public AudioSource narratorAudio;
    public NarratorSubtitlesMAIN subtitlesSystem;
    public bool playOnlyOnce = true;

    private bool hasPlayed = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && (!hasPlayed || !playOnlyOnce))
        {
            // Pokreni titlove ako postoje
            if (subtitlesSystem != null)
            {
                subtitlesSystem.StartSubtitles();
            }

            // Pokreni audio direktno ako postoji
            if (narratorAudio != null && !narratorAudio.isPlaying)
            {
                narratorAudio.Play();
            }

            hasPlayed = true;
            Debug.Log("PlateTrigger activated by: " + other.name);
        }
    }
}