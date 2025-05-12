using UnityEngine;

public class ButtonPlay : MonoBehaviour
{
    // Referenca na skriptu koja upravlja prikazom buttona
    public ButtonMenu togglerScript;

    // Referenca na skriptu koja resetira Animator komponente
    public UIAnimatorResetter animatorResetter;
    public void Resume()
    {
        // Resetiranje svih animatora
        if (animatorResetter != null)
        {
            animatorResetter.ResetAllAnimators();
        }

        // Sakrivanje buttona i nastavak igre
        if (togglerScript != null)
        {
            togglerScript.ForceHide();
        }
        else
        {
            Debug.LogWarning("Toggler script nije postavljen!");
        }
    }
}
