using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Settings")]
    public float moveSpeed = 10f;
    public float rotationSpeed = 720f;

    [Header("Controls")]
    public Joystick moveJoystick;   // Left Stick (Movement)
    public Joystick aimJoystick;    // Right Stick (Aim & Fire)

    [Header("References")]
    public Gun playerGun;
    private Rigidbody rb;
    private Vector3 movementInput;
    private Vector3 aimInput;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // 1. READ INPUT
        // Combine Keyboard + Joystick for testing
        float moveX = Input.GetAxisRaw("Horizontal") + moveJoystick.Horizontal;
        float moveZ = Input.GetAxisRaw("Vertical") + moveJoystick.Vertical;
        movementInput = new Vector3(moveX, 0f, moveZ).normalized;

        // Read Aim Input (Right Stick or Arrow Keys for PC testing)
        float aimX = aimJoystick.Horizontal;
        float aimZ = aimJoystick.Vertical;
        
        // Simple PC testing fallback (Arrow keys to aim)
        if (Input.GetKey(KeyCode.RightArrow)) aimX = 1;
        if (Input.GetKey(KeyCode.LeftArrow)) aimX = -1;
        if (Input.GetKey(KeyCode.UpArrow)) aimZ = 1;
        if (Input.GetKey(KeyCode.DownArrow)) aimZ = -1;

        aimInput = new Vector3(aimX, 0f, aimZ).normalized;

        // 2. SHOOTING LOGIC
        // If the player is pushing the Aim Joystick, we Shoot.
        if (aimInput.magnitude >= 0.1f)
        {
            if (playerGun != null) playerGun.TryShoot();
        }
    }

    void FixedUpdate()
    {
        // 3. MOVE
        if (movementInput.magnitude >= 0.1f)
        {
            rb.MovePosition(rb.position + movementInput * moveSpeed * Time.fixedDeltaTime);
        }

        // 4. ROTATE (Twin Stick Logic)
        // Priority: If aiming, look at aim direction. If NOT aiming, look at move direction.
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