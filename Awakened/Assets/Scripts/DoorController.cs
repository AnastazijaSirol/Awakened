using UnityEngine;

[RequireComponent(typeof(Animator))]
public class DoorController : MonoBehaviour
{
    [Header("Collider to disable after open")]
    public Collider blockingCollider;

    private Animator animator;
    private bool isOpen = false;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void OpenDoor()
    {
        if (isOpen) return;
        animator.SetBool("character_nearby", true);
        if (blockingCollider != null)
            blockingCollider.enabled = false;
        isOpen = true;
        
    }
}