using UnityEngine;

public class WrongButton : MonoBehaviour
{
    public void OnRestartButtonClick()
    {
        HealthManager[] managers = Object.FindObjectsByType<HealthManager>(FindObjectsSortMode.None);
        foreach (var manager in managers)
        {
            manager.LoseLife();
        }
    }
}
