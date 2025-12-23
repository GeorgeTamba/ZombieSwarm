using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Settings")]
    public float moveSpeed = 10f;
    public float rotationSpeed = 720f;

    [Header("Controls")]
    public Joystick moveJoystick;
    public Joystick aimJoystick;

    [Header("References")]
    public Gun playerGun;
    public Animator animator; // DRAG YOUR ANIMATOR HERE
    
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
        
        // Keyboard Aim fallback
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

        // 3. ANIMATION LOGIC (Crucial Step)
        UpdateAnimator();
    }

    void UpdateAnimator()
    {
        if (animator == null) return;

        // 1. Calculate Values
        bool isAiming = aimInput.magnitude > 0.1f;
        
        // Speed is 0 to 1 based on how much you push the move stick
        float currentSpeed = movementInput.magnitude; 

        // 2. Send global state to Animator
        animator.SetBool("IsAiming", isAiming);
        animator.SetFloat("Speed", currentSpeed, 0.1f, Time.deltaTime);

        // 3. Handle Strafing (Only matters if we are in Combat Mode)
        if (isAiming)
        {
            // Calculate movement relative to where we are facing
            Vector3 localMove = transform.InverseTransformDirection(movementInput);
            
            animator.SetFloat("InputX", localMove.x, 0.1f, Time.deltaTime);
            animator.SetFloat("InputZ", localMove.z, 0.1f, Time.deltaTime);
        }
    }

    void FixedUpdate()
    {
        // 4. MOVE
        if (movementInput.magnitude >= 0.1f)
        {
            rb.MovePosition(rb.position + movementInput * moveSpeed * Time.fixedDeltaTime);
        }

        // 5. ROTATE
        // Priority: Look at Aim. If not aiming, look at Movement.
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