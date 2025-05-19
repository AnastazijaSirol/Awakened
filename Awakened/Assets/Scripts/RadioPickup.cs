using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class RadioPickup : MonoBehaviour
{
    public GameObject radioUIIcon;
    public DoorController door;
    private bool picked = false;

    void Start()
    {
        GetComponent<SphereCollider>().isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (picked) return;

        if (other.CompareTag("Player"))
        {
            picked = true;

            // Animacija pickup-a
            var pc = other.GetComponent<PlayerController>();
            if (pc != null) pc.PickupRadio();

            // Inventory
            Inventory.Instance.AddItem(radioUIIcon);

            // Otvaranje vrata
            if (door != null)
                door.OpenDoor();

            // Uništi radio
            Destroy(gameObject, 0.1f);
        }
    }
}