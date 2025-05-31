using UnityEngine;

public class TerminalUIController : MonoBehaviour
{
    public GameObject terminalUI;  // UI panel koji će se prikazati

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
