using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    // Audio izvor koji će reproducirati zvuk
    private AudioSource audioSource;

    // Zvuk koji se reproducira kada se pritisne Space
    public AudioClip spaceSound;

    void Start()
    {
        // AudioSource komponenta
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlaySound();
        }
    }

    void PlaySound()
    {
        if (audioSource != null && spaceSound != null)
        {
            audioSource.PlayOneShot(spaceSound);
        }
    }
}
