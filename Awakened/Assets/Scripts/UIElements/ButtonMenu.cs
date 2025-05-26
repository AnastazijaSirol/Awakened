using UnityEngine;
using UnityEngine.UI;

public class ButtonMenu : MonoBehaviour
{
    // Tri buttona koji se prikazuju/sakrivaju
    public Button button1;
    public Button button2;
    public Button button3;

    // Toggle koji kontrolira prikaz buttona
    public Toggle toggleButton;

    // Referenca na drugu skriptu koja treba biti isključena kada je ova aktivna
    public ButtonCommands otherMenuScript;

    // Audio izvor za reproduciranje zvuka prilikom klika
    public AudioSource audioSource;

    // Zvuk koji se reproducira kada se aktivira
    public AudioClip toggleSound;

    // UI elementi koji se prikazuju zajedno s buttonima
    public GameObject buttonCommands;
    public GameObject musicText;
    public Slider musicSlider;

    void Start()
    {
        // Početno stanje toggle-a: isključeno
        toggleButton.isOn = false;

        // Početni prikaz buttona
        button1.gameObject.SetActive(true);
        button2.gameObject.SetActive(true);
        button3.gameObject.SetActive(true);

        // Početni prikaz dodatnih UI elemenata
        if (buttonCommands != null) buttonCommands.SetActive(true);
        if (musicText != null) musicText.gameObject.SetActive(true);
        if (musicSlider != null) musicSlider.gameObject.SetActive(true);

        // Promjena toggle stanja
        toggleButton.onValueChanged.AddListener(ToggleButtonsVisibility);
    }

    // Uključivanje/isključivanje toggle-a
    public void ToggleButtonsVisibility(bool isOn)
    {
        // Buttoni se prikazuju kada je toggle isključen
        bool showButtons = !isOn;

        // Prikazivanje/sakrivanje buttona ovisno o stanju toggle-a
        button1.gameObject.SetActive(showButtons);
        button2.gameObject.SetActive(showButtons);
        button3.gameObject.SetActive(showButtons);

        // Prikazivanje/sakrivanje dodatnih UI elemenata
        if (buttonCommands != null) buttonCommands.SetActive(showButtons);
        if (musicText != null) musicText.gameObject.SetActive(showButtons);
        if (musicSlider != null) musicSlider.gameObject.SetActive(showButtons);

        // Pauziranje igre kada su buttoni prikazani
        Time.timeScale = showButtons ? 0f : 1f;

        // Sakrivanje druge skripte 
        if (showButtons && otherMenuScript != null)
        {
            otherMenuScript.ForceHide();
        }

        // Reprodukcija zvuka klika
        if (audioSource != null && toggleSound != null)
        {
            audioSource.PlayOneShot(toggleSound);
        }
    }

    // Pozivanje druge skripte da prisilno zatvori ovu
    public void ForceHide()
    {
        // Uključivanje toggle-a
        toggleButton.isOn = true;

        // Osiguravanje da su svi buttoni stvarno sakriveni
        button1.gameObject.SetActive(false);
        button2.gameObject.SetActive(false);
        button3.gameObject.SetActive(false);

        // Sakrivanje dodatnih UI elemenata
        if (buttonCommands != null) buttonCommands.SetActive(false);
        if (musicText != null) musicText.gameObject.SetActive(false);
        if (musicSlider != null) musicSlider.gameObject.SetActive(false);

        // Nastavak igre
        Time.timeScale = 1f;
    }
}
