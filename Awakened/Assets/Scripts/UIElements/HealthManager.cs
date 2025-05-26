using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HealthManager : MonoBehaviour
{
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    public int lives = 3;
    public GameObject gameOverText;
    public ButtonMenu buttonMenu;

    public void LoseLife()
    {
        if (lives <= 0) return;

        lives--;

        hearts[lives].sprite = emptyHeart;

        if (lives == 0)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        if (gameOverText != null)
            gameOverText.SetActive(true);

        Invoke("ShowMenuAndRestart", 2f);
    }

    void ShowMenuAndRestart()
    {
        if (gameOverText != null)
            gameOverText.SetActive(false);

        if (buttonMenu != null)
        {
            buttonMenu.ToggleButtonsVisibility(false);
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
