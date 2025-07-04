using UnityEngine;
using UnityEngine.UI;
using TMPro; // koristi ako koristiš TMP_InputField
using UnityEngine.SceneManagement;

public class TerminalInputHandler : MonoBehaviour
{
    public TMP_InputField inputField;
    public GameObject terminalToHide;

    public NarratorSubtitlesMAIN level1Narrator;
    public AudioSource level1NarratorAudio;

    public narratorSubtitles2 level2Narrator;
    public AudioSource level2NarratorAudio;

    public narratorSubtitles2 level3Narrator;  // NOVO za Level 3
    public AudioSource level3NarratorAudio;    // NOVO za Level 3

    public GameObject finalChoicePanel; // UI panel s završnim pitanjem

    public AudioClip wrongAnswerSound;
    private AudioSource audioSource;

    private const string correctCode = "31452";

    private void Start()
    {
        if (inputField != null)
        {
            inputField.onEndEdit.AddListener(HandleInputSubmit);
        }

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        if (finalChoicePanel != null)
            finalChoicePanel.SetActive(false); // osiguraj da je skriveno na početku
    }

    private void HandleInputSubmit(string userInput)
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            CheckCode(userInput);
        }
    }

    private void CheckCode(string userInput)
    {
        if (userInput == correctCode)
        {
            if (terminalToHide != null)
                terminalToHide.SetActive(false);

            string currentScene = SceneManager.GetActiveScene().name;

            if (currentScene == "SampleScene" && level1NarratorAudio != null && !level1NarratorAudio.isPlaying)
            {
                level1NarratorAudio.Play();
                level1NarratorAudio.loop = false;
                Invoke(nameof(ChangeSceneAfterNarration), level1NarratorAudio.clip.length);

                level1Narrator?.StartSubtitles();
            }
            else if (currentScene == "Level2" && level2NarratorAudio != null && !level2NarratorAudio.isPlaying)
            {
                level2NarratorAudio.Play();
                level2NarratorAudio.loop = false;
                Invoke(nameof(ChangeSceneAfterNarration), level2NarratorAudio.clip.length);

                level2Narrator?.StartNarration();
            }
            else if (currentScene == "Level3" && level3NarratorAudio != null && !level3NarratorAudio.isPlaying)
            {
                level3NarratorAudio.Play();
                level3NarratorAudio.loop = false;
                Invoke(nameof(ShowFinalChoicePanel), level3NarratorAudio.clip.length);

                level3Narrator?.StartNarration();
            }
        }
        else
        {
            if (wrongAnswerSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(wrongAnswerSound);
            }

            HealthManager[] managers = Object.FindObjectsByType<HealthManager>(FindObjectsSortMode.None);
            foreach (var manager in managers)
            {
                manager.LoseLife();
            }

            inputField.text = "";
            inputField.ActivateInputField();
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

    private void ShowFinalChoicePanel()
    {
        if (finalChoicePanel != null)
        {
            finalChoicePanel.SetActive(true);
        }
    }
}
