using UnityEngine;

public class NarratorTrigger : MonoBehaviour
{
    public AudioSource narratorAudio;
    public NarratorSubtitlesMAIN subtitlesSystem;
    public bool playOnlyOnce = true;

    private bool hasPlayed = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && (!hasPlayed || !playOnlyOnce))
        {
            if (subtitlesSystem != null)
            {
                subtitlesSystem.StartSubtitles();
            }
            else
            {
                narratorAudio.Play();
            }

            hasPlayed = true;
        }
    }
}