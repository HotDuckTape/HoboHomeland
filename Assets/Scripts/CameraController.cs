using Unity.Netcode;
using UnityEngine;

public class CameraController : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] private Transform playerBody;

    [Header("Mouse Settings")]
    [SerializeField] private float mouseSensitivity = 100f;
    [SerializeField] private float clampAngle = 90f;
    [SerializeField] private bool invertY = false;

    [Header("Smoothing Settings")]
    [SerializeField] private bool useSmoothing = false;
    [SerializeField] [Range(0.01f, 1f)] private float smoothingFactor = 0.1f;

    private float xRotation = 0f;
    private Vector2 currentRotation;
    private Vector2 smoothRotation;

    void Start()
    {
        if (!IsOwner) return;

        LockCursor();
        currentRotation = new Vector2(playerBody.eulerAngles.y, transform.localEulerAngles.x);
    }

    void Update()
    {
        if (!IsOwner) return;

        HandleMouseLook();

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
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        mouseY = invertY ? -mouseY : mouseY;

        currentRotation.x += mouseX;
        currentRotation.y -= mouseY;

        currentRotation.y = Mathf.Clamp(currentRotation.y, -clampAngle, clampAngle);

        if (useSmoothing)
        {
            smoothRotation.x = Mathf.Lerp(smoothRotation.x, currentRotation.x, smoothingFactor);
            smoothRotation.y = Mathf.Lerp(smoothRotation.y, currentRotation.y, smoothingFactor);

            transform.localRotation = Quaternion.Euler(smoothRotation.y, 0f, 0f);
            playerBody.rotation = Quaternion.Euler(0f, smoothRotation.x, 0f);
        }
        else
        {
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
