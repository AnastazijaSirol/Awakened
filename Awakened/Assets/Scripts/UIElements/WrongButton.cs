using UnityEngine;

public class WrongButton : MonoBehaviour
{
    public AudioClip buttonClickSound;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public void OnRestartButtonClick()
    {
        // Reproduciraj zvuk ako postoji
        if (buttonClickSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(buttonClickSound);
        }

        HealthManager[] managers = Object.FindObjectsByType<HealthManager>(FindObjectsSortMode.None);
        foreach (var manager in managers)
        {
            manager.LoseLife();
        }
    }
}