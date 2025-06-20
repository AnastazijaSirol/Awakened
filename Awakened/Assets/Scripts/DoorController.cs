using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Animator))]
public class DoorController : MonoBehaviour
{
    [Header("Player settings")]
    public string playerTag = "Player";

    [Header("Collider to disable after open")]
    public Collider blockingCollider;

    [Header("Names of door parts to destroy after open")]
    public string topPartName = "door_3_top_B";
    public string bottomPartName = "door_3_bottom_B";

    private Animator animator;
    private bool isOpen = false;

    void Awake()
    {
        animator = GetComponent<Animator>();
        Collider col = GetComponent<Collider>();
        col.isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (isOpen) return;
        if (other.CompareTag(playerTag))
            OpenDoor();
    }

    private void OpenDoor()
    {
        isOpen = true;
        animator.SetBool("character_nearby", true);

        // Disable blocking collider
        if (blockingCollider != null)
            blockingCollider.enabled = false;

        // Destroy specified door parts if they exist
        DestroyChildPart(topPartName);
        DestroyChildPart(bottomPartName);
    }

    private void DestroyChildPart(string partName)
    {
        // Look for the part as a child of this door GameObject
        Transform partTransform = transform.Find(partName);
        if (partTransform != null)
            Destroy(partTransform.gameObject);
    }
}