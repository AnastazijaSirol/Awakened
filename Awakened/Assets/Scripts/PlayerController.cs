using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public float jumpForce = 8f;
    public float gravity = -9.81f;
    public float rotationSpeed = 180f;
    public float inputDeadzone = 0.1f;

    private CharacterController cc;
    private Animator animator;
    private InputSystem_Actions controls;
    private Vector2 moveInput;
    private bool sprintInput;
    private bool crouchInput;
    private bool jumpRequested;
    private Vector3 velocity;

    void Awake()
    {
        cc = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        animator.applyRootMotion = false;

        controls = new InputSystem_Actions();
        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;
        controls.Player.Sprint.performed += ctx => sprintInput = true;
        controls.Player.Sprint.canceled += ctx => sprintInput = false;
        controls.Player.Crouch.performed += ctx => crouchInput = true;
        controls.Player.Crouch.canceled += ctx => crouchInput = false;
        controls.Player.Jump.performed += ctx => jumpRequested = true;
    }

    void OnEnable() => controls.Enable();
    void OnDisable() => controls.Disable();

    void Update()
    {
        // Deadzone
        Vector2 input = moveInput.magnitude < inputDeadzone ? Vector2.zero : moveInput;

        // Camera orientation
        Vector3 camF = Camera.main.transform.forward;
        camF.y = 0;
        camF.Normalize();
        Vector3 camR = Camera.main.transform.right;
        camR.y = 0;
        camR.Normalize();

        // Horizontal movement
        float forward = input.y;
        bool isRunning = sprintInput && forward > 0f;
        float speed = isRunning ? runSpeed : walkSpeed;
        Vector3 horizontalMove = (camF * input.y + camR * input.x) * speed;

        // Vertical movement (gravity & jump)
        if (cc.isGrounded && velocity.y < 0f)
            velocity.y = 0f;
        if (jumpRequested && cc.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
            animator.SetTrigger("Jump");
        }
        jumpRequested = false;
        velocity.y += gravity * Time.deltaTime;

        // Apply movement
        Vector3 move = horizontalMove + Vector3.up * velocity.y;
        cc.Move(move * Time.deltaTime);

        // Rotation
        Vector3 flatMove = new Vector3(horizontalMove.x, 0, horizontalMove.z);
        if (flatMove.magnitude > 0.1f)
        {
            Quaternion targetRot = Quaternion.LookRotation(flatMove);
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRot,
                rotationSpeed * Time.deltaTime
            );
        }

        // Update animator parameters
        animator.SetBool("Crouch", crouchInput);
        animator.SetFloat("MoveSpeed", input.magnitude);
        animator.SetBool("IsRunning", isRunning);
        animator.SetFloat("TurnInput", input.x);
    }


    public void PickupRadio()
    {
        animator.SetTrigger("PickRadio");
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "PowerUp_Sphere")
        {
            other.gameObject.SetActive(false);
        }
    }
}