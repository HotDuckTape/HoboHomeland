using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform playerBody;  // Reference to the player's transform for horizontal rotation

    [Header("Mouse Settings")]
    [SerializeField] private float mouseSensitivity = 100f;
    [SerializeField] private float clampAngle = 90f;
    [SerializeField] private bool invertY = false;

    [Header("Smoothing Settings")]
    [SerializeField] private bool useSmoothing = false;
    [SerializeField] [Range(0.01f, 1f)] private float smoothingFactor = 0.1f; // Lower values smooth more

    private float xRotation = 0f;     // Vertical rotation stored in degrees
    private Vector2 currentRotation;  // Current rotation for smoothing purposes
    private Vector2 smoothRotation;   // Smoothed rotation values

    void Start()
    {
        LockCursor();
        // Initialize currentRotation with starting rotation values
        currentRotation = new Vector2(playerBody.eulerAngles.y, transform.localEulerAngles.x);
    }

    void Update()
    {
        HandleMouseLook();

        // Toggle cursor lock state if Escape key is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleCursorLock();
        }
    }

    /// <summary>
    /// Processes mouse inputs to control camera rotation.
    /// </summary>
    private void HandleMouseLook()
    {
        // Read mouse input; delta time is used for consistency
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Optionally invert the vertical input
        mouseY = invertY ? -mouseY : mouseY;

        // Update the current rotation values
        currentRotation.x += mouseX;
        currentRotation.y -= mouseY;

        // Clamp vertical rotation to prevent over-rotation (looking too far up or down)
        currentRotation.y = Mathf.Clamp(currentRotation.y, -clampAngle, clampAngle);

        if (useSmoothing)
        {
            // Smooth the rotation values for a more fluid camera movement
            smoothRotation.x = Mathf.Lerp(smoothRotation.x, currentRotation.x, smoothingFactor);
            smoothRotation.y = Mathf.Lerp(smoothRotation.y, currentRotation.y, smoothingFactor);

            // Apply rotations: the camera rotates vertically while the player rotates horizontally
            transform.localRotation = Quaternion.Euler(smoothRotation.y, 0f, 0f);
            playerBody.rotation = Quaternion.Euler(0f, smoothRotation.x, 0f);
        }
        else
        {
            // Apply the rotation directly without smoothing
            transform.localRotation = Quaternion.Euler(currentRotation.y, 0f, 0f);
            playerBody.rotation = Quaternion.Euler(0f, currentRotation.x, 0f);
        }
    }

    /// <summary>
    /// Locks the cursor to the center of the screen and hides it.
    /// </summary>
    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    /// <summary>
    /// Unlocks the cursor and makes it visible.
    /// </summary>
    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    /// <summary>
    /// Toggles between locked and unlocked cursor state.
    /// </summary>
    private void ToggleCursorLock()
    {
        if (Cursor.lockState == CursorLockMode.Locked)
            UnlockCursor();
        else
            LockCursor();
    }
}
