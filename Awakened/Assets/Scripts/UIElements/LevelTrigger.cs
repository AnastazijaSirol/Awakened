using TMPro;
using UnityEngine;

public class LevelTrigger : MonoBehaviour
{
    public TextMeshProUGUI levelText;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && levelText != null)
        {
            levelText.gameObject.SetActive(true); 
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && levelText != null)
        {
            levelText.gameObject.SetActive(false); 
        }
    }
}