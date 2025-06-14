using UnityEngine;

public class RobotPickup : MonoBehaviour
{
    public GameObject robotIconPrefab;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameObject newIcon = Instantiate(robotIconPrefab);
            
            Inventory.Instance.AddItem(newIcon);
            
            // Dodajemo komponentu za zaštitu igraču
            other.gameObject.AddComponent<DroneShield>();
            
            gameObject.SetActive(false);
        }
    }
}