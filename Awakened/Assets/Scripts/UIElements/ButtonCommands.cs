using UnityEngine;
using UnityEngine.UI;

public class ButtonCommands : MonoBehaviour
{
    // Referenca na button koji se prikazuje/sakriva
    public Button button1;

    // Referenca na button koji aktivira prikaz/sakrivanje
    public Button menuButton;

    // Referenca na skriptu koja se sakriva ako je ova aktivna
    public ButtonMenu otherToggleScript;

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

        // Zaustavljanje ili pokretanje vremena u igri
        Time.timeScale = buttonsVisible ? 0f : 1f;

        // Ako su ovi buttoni aktivirani, prisilno sakrij druge buttone
        if (buttonsVisible && otherToggleScript != null)
        {
            otherToggleScript.ForceHide();
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
    }
}
