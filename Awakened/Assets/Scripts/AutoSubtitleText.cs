
using TMPro;
using UnityEngine;

public class AutoSubtitleText : MonoBehaviour
{
    public void Show(string text, float duration)
    {
        StopAllCoroutines();
        gameObject.SetActive(true);
        GetComponent<TextMeshProUGUI>().text = text;
        StartCoroutine(HideAfterSeconds(duration));
    }

    private System.Collections.IEnumerator HideAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        GetComponent<TextMeshProUGUI>().text = "";
        gameObject.SetActive(false);
    }
}
