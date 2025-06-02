using UnityEngine;

public class TerminalTrigger : MonoBehaviour
{
    public TerminalUIController terminal;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            terminal.ShowUI();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            terminal.HideUI();
        }
    }
}