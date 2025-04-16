using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : NetworkBehaviour
{
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 10f;
    [SerializeField] private float gravity = -9.81f;

    private CharacterController controller;
    private Vector3 velocity;
    private Animator animator;

    void Start()
    {
        // Only run code for the local player.
        if (!IsOwner) return;

        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        // Hide the character model so the local player does not see their own body.
        HideLocalPlayerModel();
    }

    void Update()
    {
        if (!IsOwner) return;

        HandleMovement();
        ApplyGravity();
    }

    private void HandleMovement()
    {
        Vector3 inputDirection = GetInputDirection();
        float currentSpeed = GetCurrentSpeed();

        if (inputDirection.sqrMagnitude < 0.01f)
        {
            animator.SetFloat("Speed", 0f);
            return;
        }

        Vector3 move = transform.right * inputDirection.x + transform.forward * inputDirection.z;
        controller.Move(move * currentSpeed * Time.deltaTime);

        if (currentSpeed == walkSpeed)
        {
            animator.SetFloat("Speed", 0.3f);
        }
        else if (currentSpeed == runSpeed)
        {
            animator.SetFloat("Speed", 0.6f);
        }
    }

    private Vector3 GetInputDirection()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        return new Vector3(moveX, 0, moveZ);
    }

    private float GetCurrentSpeed()
    {
        return Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
    }

    private void ApplyGravity()
    {
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void HideLocalPlayerModel()
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer rend in renderers)
        {
            if (rend.gameObject.CompareTag("Holdable"))
                continue;

            rend.enabled = false;
        }
    }

}
