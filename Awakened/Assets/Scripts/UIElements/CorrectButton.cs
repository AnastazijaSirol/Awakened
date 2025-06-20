using UnityEngine;
using UnityEngine.SceneManagement;

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
            narratorAudio.loop = false;
            Invoke(nameof(ChangeSceneAfterNarration), narratorAudio.clip.length);
        }

        if (backstoryNarrator != null)
        {
            backstoryNarrator.StartSubtitles();
        }
    }

    private void ChangeSceneAfterNarration()
    {
        if (SceneManager.GetActiveScene().name == "SampleScene")
        {
            SceneManager.LoadScene("Level2");
        }
        else if (SceneManager.GetActiveScene().name == "Level2")
        {
            SceneManager.LoadScene("Level3");
        }
    }
}
