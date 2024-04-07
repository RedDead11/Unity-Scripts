using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float sprintSpeed;
    [SerializeField] private float jumpHeight;


    [Header("Mouse Settings")]
    [SerializeField] private float sensitivity;


    [Tooltip("How much player can loop up (use -60f)")]
    [SerializeField] private float limitLookX;

    [Tooltip("How much player can loop down, (use 60f)")]
    [SerializeField] private float limitLookY;


    private Rigidbody rb;
    private bool isGrounded;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        MouseLook();
        Movement();
        HandleJump();
    }

    void Movement()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        if (horizontalInput != 0f || verticalInput != 0f)
        {
            // Calculate movement direction based on player input
            Vector3 forwardDirection = transform.forward;
            forwardDirection.y = 0f;
            forwardDirection = forwardDirection.normalized;

            Vector3 rightDirection = transform.right;

            // Combine input directions to get the movement direction
            Vector3 movementDirection = (forwardDirection * verticalInput + rightDirection * horizontalInput).normalized;

            // Apply movement speed to the movement direction
            if(Input.GetKey(KeyCode.LeftShift)) 
                rb.velocity = movementDirection * sprintSpeed;
            else
                rb.velocity = movementDirection * moveSpeed;
        }
        else
        {
            // If no movement keys are pressed, stop the player's movement
            rb.velocity = Vector3.zero;
        }
    }


    void MouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        // Rotate horizontally (around Y axis)
        transform.Rotate(Vector3.up * mouseX);

        // Get current rotation values
        Vector3 currentRotation = transform.rotation.eulerAngles;

        // Calculate new vertical rotation after applying mouseY
        float newVerticalRotation = currentRotation.x - mouseY;

        // Clamp the vertical rotation within a specified range
        float minVerticalAngle = limitLookX; // Set your minimum vertical angle here
        float maxVerticalAngle = limitLookY;  // Set your maximum vertical angle here

        // Normalize the angle between -180° and 180° to prevent angle wrapping issues
        newVerticalRotation = NormalizeAngle(newVerticalRotation);

        // Clamp the normalized angle within the specified range
        newVerticalRotation = Mathf.Clamp(newVerticalRotation, minVerticalAngle, maxVerticalAngle);

        // Apply the new rotation
        transform.rotation = Quaternion.Euler(newVerticalRotation, currentRotation.y, 0f);
    }

    // Helper function to normalize angles between -180° and 180°
    float NormalizeAngle(float angle)
    {
        while (angle > 180f)
        {
            angle -= 360f;
        }

        while (angle < -180f)
        {
            angle += 360f;
        }

        return angle;
    }



    void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            // Calculate jump velocity
            Vector3 jumpVelocity = Vector3.up * Mathf.Sqrt(2f * jumpHeight * Mathf.Abs(Physics.gravity.y));
            rb.velocity += jumpVelocity;

            isGrounded = false; // Player is no longer grounded after jump
        }

        // Check if player released jump key to limit jump height
        if (Input.GetKeyUp(KeyCode.Space) && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        // Check if the player is grounded by checking collision normals
        foreach (ContactPoint contact in collision.contacts)
        {
            if (Vector3.Dot(contact.normal, Vector3.up) > 0.7f)
            {
                isGrounded = true;
                break;
            }
        }
    }
}
