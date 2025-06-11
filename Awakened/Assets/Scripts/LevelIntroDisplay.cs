using UnityEngine;
using TMPro;
using System.Collections;

public class LevelIntroDisplay : MonoBehaviour
{
    public TextMeshProUGUI levelText;
    public CanvasGroup canvasGroup;

    public void ShowLevelText(string message, float displayTime = 3f, float fadeTime = 1f)
    {
        StartCoroutine(FadeTextRoutine(message, displayTime, fadeTime));
    }

    private IEnumerator FadeTextRoutine(string msg, float displayTime, float fadeTime)
    {
        levelText.text = msg;
        canvasGroup.alpha = 0;
        levelText.gameObject.SetActive(true);

        float t = 0;
        while (t < fadeTime)
        {
            canvasGroup.alpha = Mathf.Lerp(0, 1, t / fadeTime);
            t += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 1;

        yield return new WaitForSeconds(displayTime);

        t = 0;
        while (t < fadeTime)
        {
            canvasGroup.alpha = Mathf.Lerp(1, 0, t / fadeTime);
            t += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 0;

        levelText.gameObject.SetActive(false);
    }
}