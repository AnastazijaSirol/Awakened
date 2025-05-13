using UnityEngine;
using UnityEngine.UI;

public class ButtonCommands : MonoBehaviour
{
    // Referenca na button koji se prikazuje/sakrivaju
    public Button button1;

    // Referenca na button koji aktivira prikaz/sakrivanje
    public Button menuButton;

    // Referenca na skriptu koja se sakriva ako je ova aktivna
    public ButtonMenu otherToggleScript;

    // Referenca na skriptu koja resetira animatore
    public UIAnimatorResetter animatorResetter;

    // Trenutno stanje buttona
    private bool buttonsVisible = false;

    // Audio komponenta za zvuk klika
    public AudioSource audioSource;

    // Zvuk koji će se pustiti prilikom klika
    public AudioClip clickSound;

    void Start()
    {
        // Na početku su sakriveni
        SetButtonsActive(buttonsVisible);

        // Aktivacija kada se klikne button
        menuButton.onClick.AddListener(ToggleButtonsVisibility);
    }

    // Mijenjanje trenutnog stanja prikaza buttona
    private void ToggleButtonsVisibility()
    {
        // Puštanje zvuka klika ako je sve postavljeno
        if (audioSource != null && clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
        }

        // Prebacivanje stanja
        buttonsVisible = !buttonsVisible;

        // Primjena novog stanja
        SetButtonsActive(buttonsVisible);

        // Pauziranje ili nastavak igre
        Time.timeScale = buttonsVisible ? 0f : 1f;

        // Ako su ovi buttoni aktivirani, prisilno sakrij druge buttone
        if (buttonsVisible && otherToggleScript != null)
        {
            otherToggleScript.ForceHide();
        }

        // Reset animatora
        if (animatorResetter != null)
        {
            animatorResetter.ResetAllAnimators();
        }
    }

    // Aktivira ili deaktivira button ovisno o primljenom stanju
    private void SetButtonsActive(bool state)
    {
        button1.gameObject.SetActive(state);
    }

    // Metoda kojom skripte mogu prisilno sakriti ove buttone
    public void ForceHide()
    {
        buttonsVisible = false;
        SetButtonsActive(false);
        Time.timeScale = 1f;

        // Reset animatora
        if (animatorResetter != null)
        {
            animatorResetter.ResetAllAnimators();
        }
    }

    public void Resume()
    {
        // Resetiranje svih animatora
        if (animatorResetter != null)
        {
            animatorResetter.ResetAllAnimators();
        }
    }
}
