using UnityEngine;
using TMPro;

public class CountdownTimer : MonoBehaviour
{
    public float countdownDuration = 30f;
    private float timeRemaining;
    private bool isCounting = false;

    public TextMeshProUGUI uiText;

    void OnEnable()
    {
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
        }
    }

    void UpdateDisplay()
    {
        int seconds = Mathf.CeilToInt(timeRemaining);
        uiText.text = "Vrijeme: " + seconds.ToString() + "s";
    }
}
