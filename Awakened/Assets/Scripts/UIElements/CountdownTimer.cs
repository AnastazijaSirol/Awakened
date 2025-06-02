using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class CountdownTimer : MonoBehaviour
{
    public float countdownDuration = 30f;
    private float timeRemaining;
    private bool isCounting = false;

    public TextMeshProUGUI uiText;
    public List<HealthManager> healthManagers = new List<HealthManager>();

    [Header("Terminal koji će se isključiti kad vrijeme istekne")]
    public GameObject terminalToHide;

    void OnEnable()
    {
        if (healthManagers.Count == 0)
        {
            healthManagers.AddRange(
                Object.FindObjectsByType<HealthManager>(FindObjectsSortMode.None)
            );
        }

        timeRemaining = countdownDuration;
        isCounting = true;
    }

    void Update()
    {
        if (isCounting && timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            UpdateDisplay();
        }
        else if (isCounting && timeRemaining <= 0)
        {
            timeRemaining = 0;
            isCounting = false;
            UpdateDisplay();

            // Pozovi Die() na svim HealthManagerima
            foreach (var manager in healthManagers)
            {
                if (manager != null)
                {
                    manager.Die();
                }
            }

            // Sakrij terminal ako postoji
            if (terminalToHide != null)
            {
                terminalToHide.SetActive(false);
            }
        }
    }

    void UpdateDisplay()
    {
        int seconds = Mathf.CeilToInt(timeRemaining);
        uiText.text = "Vrijeme: " + seconds.ToString() + "s";
    }
}
