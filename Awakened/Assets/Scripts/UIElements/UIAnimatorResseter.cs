using UnityEngine;

public class UIAnimatorResetter : MonoBehaviour
{
    // Niz Animator komponenti koje treba resetirati
    public Animator[] animators;

    // Metoda za resetiranje svih Animator komponenti u nizu
    public void ResetAllAnimators()
    {
        foreach (var animator in animators)
        {
            // Vraćanje animatora u početno stanje
            animator.Rebind();

            // Ažuriranje animatora kako bi se odmah primijenile promjene
            animator.Update(0f);

            // Ponovno pokretanje zadane animacije
            animator.Play("SF Button", 0);
        }
    }
}
