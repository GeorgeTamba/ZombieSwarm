using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Settings")]
    public float moveSpeed = 10f;
    public float rotationSpeed = 720f;
    
    // NEW: How fast we move when aiming (0.5 = 50% speed)
    [Range(0.1f, 1f)]
    public float aimMovementPenalty = 0.5f; 

    [Header("Controls")]
    public Joystick moveJoystick;
    public Joystick aimJoystick;

    [Header("References")]
    public Gun playerGun;
    public Animator animator; 
    
    private Rigidbody rb;
    private Vector3 movementInput;
    private Vector3 aimInput;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // 1. INPUT
        float moveX = Input.GetAxisRaw("Horizontal") + moveJoystick.Horizontal;
        float moveZ = Input.GetAxisRaw("Vertical") + moveJoystick.Vertical;
        movementInput = new Vector3(moveX, 0f, moveZ).normalized;

        float aimX = aimJoystick.Horizontal;
        float aimZ = aimJoystick.Vertical;
        
        // PC Testing Fallback
        if (Input.GetKey(KeyCode.RightArrow)) aimX = 1;
        if (Input.GetKey(KeyCode.LeftArrow)) aimX = -1;
        if (Input.GetKey(KeyCode.UpArrow)) aimZ = 1;
        if (Input.GetKey(KeyCode.DownArrow)) aimZ = -1;

        aimInput = new Vector3(aimX, 0f, aimZ).normalized;

        // 2. SHOOTING
        if (aimInput.magnitude >= 0.1f)
        {
            if (playerGun != null) playerGun.TryShoot();
        }

        // 3. ANIMATION
        UpdateAnimator();
    }

    void UpdateAnimator()
    {
        if (animator == null) return;

        bool isAiming = aimInput.magnitude > 0.1f;
        float currentSpeed = movementInput.magnitude;

        animator.SetBool("IsAiming", isAiming);
        animator.SetFloat("Speed", currentSpeed, 0.1f, Time.deltaTime);

        if (isAiming)
        {
            Vector3 localMove = transform.InverseTransformDirection(movementInput);
            animator.SetFloat("InputX", localMove.x, 0.1f, Time.deltaTime);
            animator.SetFloat("InputZ", localMove.z, 0.1f, Time.deltaTime);
        }
    }

    void FixedUpdate()
    {
        // 4. MOVE (UPDATED LOGIC)
        if (movementInput.magnitude >= 0.1f)
        {
            // Start with base speed
            float currentMoveSpeed = moveSpeed;

            // If we are aiming, reduce the speed
            if (aimInput.magnitude >= 0.1f)
            {
                currentMoveSpeed *= aimMovementPenalty;
            }

            // Apply movement
            rb.MovePosition(rb.position + movementInput * currentMoveSpeed * Time.fixedDeltaTime);
        }

        // 5. ROTATE
        if (aimInput.magnitude >= 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(aimInput);
            rb.rotation = Quaternion.RotateTowards(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }
        else if (movementInput.magnitude >= 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movementInput);
            rb.rotation = Quaternion.RotateTowards(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }
    }
}