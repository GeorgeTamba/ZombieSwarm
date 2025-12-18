using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Settings")]
    public float moveSpeed = 10f;
    public float rotationSpeed = 720f;

    [Header("Controls")]
    public Joystick joystick;       // Drag your On-Screen Joystick here
    public HoldButton fireButton;   // Drag your On-Screen Fire Button here

    [Header("References")]
    public Gun playerGun;
    private Rigidbody rb;
    private Vector3 movementInput;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // 1. INPUT (Combine Keyboard + Joystick for easy testing)
        // This allows you to test on PC with WASD *AND* on phone with Joystick
        float moveX = Input.GetAxisRaw("Horizontal") + joystick.Horizontal;
        float moveZ = Input.GetAxisRaw("Vertical") + joystick.Vertical;

        movementInput = new Vector3(moveX, 0f, moveZ).normalized;

        // 2. SHOOTING (Combine Spacebar + Mobile Button)
        bool isFiring = Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0);
        
        // Add the mobile check
        if (fireButton != null && fireButton.isPressed)
        {
            isFiring = true;
        }

        if (isFiring && playerGun != null)
        {
            playerGun.TryShoot();
        }
    }

    void FixedUpdate()
    {
        // 3. APPLY MOVEMENT
        if (movementInput.magnitude >= 0.1f)
        {
            rb.MovePosition(rb.position + movementInput * moveSpeed * Time.fixedDeltaTime);

            Quaternion targetRotation = Quaternion.LookRotation(movementInput);
            rb.rotation = Quaternion.RotateTowards(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }
    }
}