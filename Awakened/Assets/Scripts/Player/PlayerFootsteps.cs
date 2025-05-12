using UnityEngine;

public class PlayerFootsteps : MonoBehaviour
{
    // AudioSource za reprodukciju zvukova koraka
    private AudioSource footstepAudioSource;

    // Zvuk koraka
    public AudioClip footstepSound;

    // Brzina hodanja
    public float walkSpeed = 1.0f;

    // Broj sekundi između zvukova koraka
    private float stepInterval;

    // Trenutni interval između zvukova koraka
    private float stepTimer;

    void Start()
    {
        // AudioSource komponenta
        footstepAudioSource = GetComponent<AudioSource>();

        // Interval između koraka na temelju brzine hodanja
        stepInterval = 1f / walkSpeed;
    }

    void Update()
    {
        // Timer za interval
        stepTimer += Time.deltaTime;

        // Je li igrač u pokretu
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            // Ako je prošlo dovoljno vremena, reproduciranje zvuka koraka
            if (stepTimer >= stepInterval)
            {
                PlayFootstepSound();
                stepTimer = 0f;
            }
        }
        else
        {
            stepTimer = 0f;
        }
    }

    void PlayFootstepSound()
    {
        // Reproduciranje zvuka koraka
        if (footstepAudioSource != null && footstepSound != null)
        {
            footstepAudioSource.PlayOneShot(footstepSound);
        }
    }
}
