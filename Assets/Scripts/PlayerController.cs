using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Settings")]
    public float moveSpeed = 10f;
    public float rotationSpeed = 720f;

    [Header("Dependencies")]
    public Gun playerGun; // Reference to the Gun script

    private Rigidbody rb;
    private Vector3 movementInput;
    private Vector3 aimInput;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // Automatically find the gun if you forgot to drag it in
        if (playerGun == null) playerGun = GetComponent<Gun>();
    }

    void Update()
    {
        // ... (Keep your existing WASD movement input code here) ...
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");
        movementInput = new Vector3(moveX, 0f, moveZ).normalized;

        // ... (Keep your existing Aim input code here) ...
        // For testing aiming with arrow keys:
        float aimX = 0;
        float aimZ = 0;
        if(Input.GetKey(KeyCode.RightArrow)) aimX = 1;
        if(Input.GetKey(KeyCode.LeftArrow)) aimX = -1;
        if(Input.GetKey(KeyCode.UpArrow)) aimZ = 1;
        if(Input.GetKey(KeyCode.DownArrow)) aimZ = -1;
        aimInput = new Vector3(aimX, 0f, aimZ).normalized;


        // NEW: SHOOTING INPUT
        // For now, Spacebar to shoot. Later we bind this to the joystick.
        if (Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0)) 
        {
            playerGun.TryShoot();
        }
    }

    void FixedUpdate()
    {
        // ... (Keep your existing FixedUpdate movement/rotation code) ...
        if (movementInput.magnitude >= 0.1f)
        {
            rb.MovePosition(rb.position + movementInput * moveSpeed * Time.fixedDeltaTime);
        }

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