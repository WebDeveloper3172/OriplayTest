using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Vector2 moveInput;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        // Subscribe to the input system
        var playerInput = new PlayerInput();
        playerInput.Player.Move.performed += OnMove;
        playerInput.Player.Move.canceled += OnMove;
        playerInput.Enable();
    }

    private void OnDisable()
    {
        // Unsubscribe from the input system
        var playerInput = new PlayerInput();
        playerInput.Player.Move.performed -= OnMove;
        playerInput.Player.Move.canceled -= OnMove;
        playerInput.Disable();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    private void Update()
    {
        // Apply the movement input to the player's position
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y) * moveSpeed * Time.deltaTime;
        transform.Translate(move, Space.World);

        if (IsMoving())
        {
            animator.SetBool("isRunning", true );
        }
        else
        {
            animator.SetBool("isRunning", false);
        }
    }
    public bool IsMoving()
    {
        return moveInput != Vector2.zero;
    }
}
