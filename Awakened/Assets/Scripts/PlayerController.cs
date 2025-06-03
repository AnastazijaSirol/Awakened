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

    [Header("Crouch Settings")]
    public float crouchHeight = 1.0f;    // CharacterController height when crouching
    public float standHeight = 2.0f;     // CharacterController height when standing

    private CharacterController cc;
    private Animator animator;
    private InputSystem_Actions controls;
    private Vector2 moveInput;
    private bool sprintInput;
    private bool crouchInput;
    private bool jumpRequested;
    private Vector3 velocity;

    // Track previous crouch state to apply height adjustment only on toggle
    private bool wasCrouching = false;

    void Awake()
    {
        cc = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        animator.applyRootMotion = false;

        // Initialize CharacterController to standing dimensions
        cc.height = standHeight;
        cc.center = new Vector3(0f, standHeight / 2f, 0f);

        controls = new InputSystem_Actions();
        controls.Player.Move.performed   += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled    += ctx => moveInput = Vector2.zero;
        controls.Player.Sprint.performed += ctx => sprintInput = true;
        controls.Player.Sprint.canceled  += ctx => sprintInput = false;
        controls.Player.Crouch.performed += ctx => crouchInput = true;
        controls.Player.Crouch.canceled  += ctx => crouchInput = false;
        controls.Player.Jump.performed   += ctx => jumpRequested = true;
    }

    void OnEnable()  => controls.Enable();
    void OnDisable() => controls.Disable();

    void Update()
    {
        // Deadzone for input
        Vector2 input = moveInput.magnitude < inputDeadzone ? Vector2.zero : moveInput;

        // Handle crouch toggle: adjust height & keep bottom at same Y
        if (crouchInput != wasCrouching)
        {
            // Calculate current bottom Y of CharacterController
            float oldBottomY = transform.position.y + cc.center.y - (cc.height / 2f);

            // Set new height and center depending on crouch state
            float targetHeight = crouchInput ? crouchHeight : standHeight;
            cc.height = targetHeight;
            cc.center = new Vector3(0f, targetHeight / 2f, 0f);

            // Adjust transform.position.y so bottom stays at oldBottomY
            transform.position = new Vector3(
                transform.position.x,
                oldBottomY,
                transform.position.z
            );

            wasCrouching = crouchInput;
        }

        // Camera orientation on XZ plane
        Vector3 camF = Camera.main.transform.forward;
        camF.y = 0f; camF.Normalize();
        Vector3 camR = Camera.main.transform.right;
        camR.y = 0f; camR.Normalize();

        // Horizontal movement (no speed reduction when crouching)
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

        // Rotation toward movement direction
        Vector3 flatMove = new Vector3(horizontalMove.x, 0f, horizontalMove.z);
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Terminal"))
        {
            animator.SetTrigger("PressButton");
        }
    }

    public void PickupRadio()
    {
        animator.SetTrigger("PickRadio");
    }
}