using UnityEngine;

public class CorrectButton : MonoBehaviour
{
    public GameObject terminalToHide;

    public void OnCorrectButtonClick()
    {
        if (terminalToHide != null)
        {
            terminalToHide.SetActive(false);
        }
    }
}
