using UnityEngine;

public class CorrectButton : MonoBehaviour
{
    public GameObject terminalToHide;
    public NarratorSubtitlesMAIN backstoryNarrator;
    public AudioSource narratorAudio;

    public void OnCorrectButtonClick()
    {
        if (terminalToHide != null)
        {
            terminalToHide.SetActive(false);
        }

        if (narratorAudio != null && !narratorAudio.isPlaying)
        {
            narratorAudio.Play();
        }

        if (backstoryNarrator != null)
        {
            backstoryNarrator.StartSubtitles();
        }
    }
}