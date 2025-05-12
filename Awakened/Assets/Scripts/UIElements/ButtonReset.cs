using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ButtonReset : MonoBehaviour
{
    // Referenca na skriptu
    public ButtonMenu togglerScript;

    // Referenca na skriptu za resetiranje Animator komponenti
    public UIAnimatorResetter animatorResetter;

    // Audio izvor za zvuk
    public AudioSource audioSource;

    // Zvuk koji će se reproducirati prilikom klika
    public AudioClip clickSound;

    // Trajanje čekanja prije resetiranja scene
    public float delayBeforeReload = 0.3f;

    // Metoda za ponovno učitavanje trenutne scene
    public void ResetScene()
    {
        // Resetiranje svih aktivnih Animator komponenti
        if (animatorResetter != null)
        {
            animatorResetter.ResetAllAnimators();
        }

        // Reproduciranje zvuka i restartanje scene
        StartCoroutine(PlaySoundAndReload());
    }

    private IEnumerator PlaySoundAndReload()
    {
        if (audioSource != null && clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
        }

        // Čekanje da se zvuk reproducira
        yield return new WaitForSecondsRealtime(delayBeforeReload);

        // Učitavanje trenutne scene
        string currentScene = SceneManager.GetActiveScene().name;

        // Učitavanje scene
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(currentScene);
        if (asyncLoad == null)
        {
            Debug.LogError("Neuspješno učitavanje scene: " + currentScene);
        }
        else
        {
            // Učitavanje scene u pozadini
            asyncLoad.allowSceneActivation = true;
        }
    }
}
