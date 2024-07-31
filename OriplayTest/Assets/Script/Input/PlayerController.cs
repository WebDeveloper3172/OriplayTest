using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotatespeed;
    public VariableJoystick variableJoystick;
    public Rigidbody rb;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        // Obține direcția de mișcare de la joystick
        Vector3 direction = Vector3.forward * variableJoystick.Vertical + Vector3.right * variableJoystick.Horizontal;

        direction = direction.normalized;
        Vector3 targetVelocity = direction * moveSpeed;
        // Aplică forța pe Rigidbody pentru a mișca jucătorul
        rb.velocity = new Vector3(targetVelocity.x, rb.velocity.y, targetVelocity.z);

        //if (direction.sqrMagnitude <= 0)
        //{
        //    return;
        //}

        var targetDirection = Vector3.RotateTowards(transform.forward, direction, rotatespeed * Time.deltaTime, 0.0f);
        transform.rotation = Quaternion.LookRotation(targetDirection);

        // Actualizează animația în funcție de mișcare
        if (IsMoving())
        {
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }
    }

    public bool IsMoving()
    {
        return variableJoystick.Horizontal != 0 || variableJoystick.Vertical != 0;
    }
}
