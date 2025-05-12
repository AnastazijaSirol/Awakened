using UnityEngine;
using UnityEngine.UI;

public class ButtonQuit : MonoBehaviour
{
    // Panel koji se prikazuje kada korisnik želi izaći iz igre
    public GameObject quitPanel;

    // Button za prikaz naredbi koji se deaktivira kada je panel aktivan
    public Button commandsButton;

    // Toggle za meni koji se deaktivira kada je panel aktivan
    public Toggle menuToggle;

    // Button za potvrdu izlaska
    public Button yesButton;

    // Button za odustajanje od izlaska
    public Button noButton;

    // Skripta koja resetira Animator komponente na UI elementima
    public UIAnimatorResetter animatorResetter;

    void Start()
    {
        // Button "Yes" (izlazak iz igre)
        if (yesButton != null) 
            yesButton.onClick.AddListener(QuitGame);

        // Button "No" (sakrivanje panela)
        if (noButton != null) 
            noButton.onClick.AddListener(HideQuitPanel);
    }

    // Prikazivanje quit panela i pauziranje igre
    public void ShowQuitPanel()
    {
        // Resetiranje UI animacije
        if (animatorResetter != null)
        {
            animatorResetter.ResetAllAnimators();
        }

        // Panel za potvrdu izlaska
        quitPanel.SetActive(true);

        // Pauziranje igre
        Time.timeScale = 0f;

        // Onemogućavanje drugih kontrola dok je panel otvoren
        if (commandsButton != null) commandsButton.interactable = false;
        if (menuToggle != null) menuToggle.interactable = false;
    }

    public void HideQuitPanel()
    {
        // Resetiranje UI animacija
        if (animatorResetter != null)
        {
            animatorResetter.ResetAllAnimators();
        }

        // Sakrivanje panel
        quitPanel.SetActive(false);

        // Nastavak igre
        Time.timeScale = 1f;

        // Ponovno omogućavanje kontrola
        if (commandsButton != null) commandsButton.interactable = true;
        if (menuToggle != null) menuToggle.interactable = true;
    }

    // Potvrda izlaska iz igre
    public void QuitGame()
    {
        // Resetiranje UI animacije
        if (animatorResetter != null)
        {
            animatorResetter.ResetAllAnimators();
        }

        Debug.Log("Izlazimo iz igre...");

        // Zatvaranje aplikacije
        Application.Quit();
    }
}
