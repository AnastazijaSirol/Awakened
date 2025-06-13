using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HealthManager : MonoBehaviour
{
    public static event Action OnLifeLost;

    [Header("UI Hearts")]
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    [Header("Game Over UI")]
    public GameObject gameOverText;
    public GameObject terminalUI; // Referenca na terminal UI
    public ButtonMenu buttonMenu;

    [Header("Lives Settings")]
    public int lives = 3;

    [Header("Animator for Death Animation")]
    public Animator playerAnimator;
    public string dieTriggerName = "IsDead";

    private bool isDead = false;

    void Start()
    {
        // Inicijalno sakrij Game Over tekst
        if (gameOverText != null)
            gameOverText.SetActive(false);
    }

    public void LoseLife()
    {
        if (isDead) return;
        if (lives <= 0) return;

        lives--;
        OnLifeLost?.Invoke();

        // Ažuriraj srca
        if (lives >= 0 && lives < hearts.Length)
        {
            hearts[lives].sprite = emptyHeart;
        }

        // Kada potroši sva srca
        if (lives == 0)
        {
            Die();
        }
    }

    public void Die()
    {
        isDead = true;

        // Pokreni animaciju smrti
        if (playerAnimator != null && !string.IsNullOrEmpty(dieTriggerName))
        {
            playerAnimator.SetTrigger(dieTriggerName);
        }

        // Sakrij terminal
        if (terminalUI != null)
        {
            terminalUI.SetActive(false);
        }

        // Prikaži Game Over tekst
        if (gameOverText != null)
        {
            gameOverText.SetActive(true);
        }
        
        Invoke(nameof(ShowMenuAndRestart), 2.5f);
    }

    private void ShowMenuAndRestart()
    {
        if (gameOverText != null)
            gameOverText.SetActive(false);

        if (buttonMenu != null)
        {
            buttonMenu.ToggleButtonsVisibility(true); 
        }
    }
}