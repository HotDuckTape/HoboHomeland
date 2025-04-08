using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 10f;
    [SerializeField] private float gravity = -9.81f;

    private CharacterController controller;
    private Vector3 velocity;

    void Start()
    {
        if (controller == null)
        {
            controller = GetComponent<CharacterController>();
        }
    }

    void Update()
    {
        HandleMovement();
        ApplyGravity();
    }

    private void HandleMovement()
    {
        Vector3 inputDirection = GetInputDirection();
        float currentSpeed = GetCurrentSpeed();

        Vector3 move = transform.right * inputDirection.x + transform.forward * inputDirection.z;
        controller.Move(move * currentSpeed * Time.deltaTime);
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
}
