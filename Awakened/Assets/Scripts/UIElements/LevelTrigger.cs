using UnityEngine;
using TMPro;

public class LevelTrigger : MonoBehaviour
{
    public TextMeshProUGUI levelText;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (levelText != null)
            {
                levelText.text = "1. level";
                levelText.gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (levelText != null)
            {
                levelText.gameObject.SetActive(false);
            }
        }
    }

}
