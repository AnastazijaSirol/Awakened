using UnityEngine;

public class TerminalUIController : MonoBehaviour
{
    public GameObject terminalUI;

    public void ShowUI()
    {
        if (terminalUI != null)
            terminalUI.SetActive(true);
    }

    public void HideUI()
    {
        if (terminalUI != null)
            terminalUI.SetActive(false);
    }
}