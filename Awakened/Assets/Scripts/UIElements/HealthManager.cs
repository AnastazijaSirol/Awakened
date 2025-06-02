using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HealthManager : MonoBehaviour
{
    [Header("UI Hearts")]
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    [Header("Game Over UI")]
    public GameObject gameOverText;
    public ButtonMenu buttonMenu;

    [Header("Lives Settings")]
    public int lives = 3;

    [Header("Animator for Death Animation")]
    public Animator playerAnimator;
    public string dieTriggerName = "IsDead";

    private bool isDead = false;

    public void LoseLife()
    {
        if (isDead) return;       // If already dead, no more lives lost
        if (lives <= 0) return;

        lives--;

        // Show 1 less heart
        if (lives >= 0 && lives < hearts.Length)
        {
            hearts[lives].sprite = emptyHeart;
        }

        // If all lives are lost, player dies
        if (lives == 0)
        {
            Die();
        }
    }

    public void Die()
    {
        isDead = true;

        // Start dying animation
        if (playerAnimator != null && !string.IsNullOrEmpty(dieTriggerName))
        {
            playerAnimator.SetTrigger(dieTriggerName);
        }

        // Show “Game Over” text
        if (gameOverText != null)
        {
            gameOverText.SetActive(true);
        }

        // Restart scene after 2 seconds (or main menu)
        Invoke(nameof(ShowMenuAndRestart), 2f);
    }

    private void ShowMenuAndRestart()
    {
        if (gameOverText != null)
            gameOverText.SetActive(false);

        if (buttonMenu != null)
        {
            buttonMenu.ToggleButtonsVisibility(false);
        }

        // Restart scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}