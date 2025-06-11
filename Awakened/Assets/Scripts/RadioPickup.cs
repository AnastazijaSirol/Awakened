using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class RadioPickup : MonoBehaviour
{
    public GameObject radioUIIcon;
    public DoorControllerRadio door;

    public AudioSource narratorAudio;
    public NarratorSubtitlesMAIN subtitles;
    public GameObject radioVisual;

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

            if (narratorAudio != null)
                narratorAudio.Play();

            if (subtitles != null)
                subtitles.StartSubtitles();

            var pc = other.GetComponent<PlayerController>();
            if (pc != null) pc.PickupRadio();

            Inventory.Instance.AddItem(radioUIIcon);

            if (door != null)
                door.OpenDoor();

            if (radioVisual != null)
                radioVisual.SetActive(false);
        }
    }
}
