using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Animator))]
public class DoorController : MonoBehaviour
{
    [Header("Player settings")]
    public string playerTag = "Player";

    [Header("Collider to disable after open")]
    public Collider blockingCollider;

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
        if (blockingCollider != null)
            blockingCollider.enabled = false;
    }
}