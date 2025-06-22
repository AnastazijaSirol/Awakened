using UnityEngine;
using UnityEngine.SceneManagement;

public class CorrectButton : MonoBehaviour
{
    public GameObject terminalToHide;

    // Level 1 naracija
    public NarratorSubtitlesMAIN level1Narrator;
    public AudioSource level1NarratorAudio;

    // Level 2 naracija
    public narratorSubtitles2 level2Narrator;
    public AudioSource level2NarratorAudio;

    public void OnCorrectButtonClick()
    {
        if (terminalToHide != null)
        {
            terminalToHide.SetActive(false);
        }

        string currentScene = SceneManager.GetActiveScene().name;

        if (currentScene == "SampleScene" && level1NarratorAudio != null && !level1NarratorAudio.isPlaying)
        {
            level1NarratorAudio.Play();
            level1NarratorAudio.loop = false;
            Invoke(nameof(ChangeSceneAfterNarration), level1NarratorAudio.clip.length);

            if (level1Narrator != null)
                level1Narrator.StartSubtitles();
        }
        else if (currentScene == "Level2" && level2NarratorAudio != null && !level2NarratorAudio.isPlaying)
        {
            level2NarratorAudio.Play();
            level2NarratorAudio.loop = false;
            Invoke(nameof(ChangeSceneAfterNarration), level2NarratorAudio.clip.length);

            if (level2Narrator != null)
                level2Narrator.StartNarration();
        }
    }

    private void ChangeSceneAfterNarration()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        if (currentScene == "SampleScene")
        {
            SceneManager.LoadScene("Level2");
        }
        else if (currentScene == "Level2")
        {
            SceneManager.LoadScene("Level3");
        }
    }
}
