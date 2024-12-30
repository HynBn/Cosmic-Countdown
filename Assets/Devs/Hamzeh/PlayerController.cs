using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float gravityStrength = 20f;

    [Header("Surface Detection")]
    [SerializeField] private float gravityCheckRadius = 2f;
    [SerializeField] private LayerMask surfaceLayers;

    [Header("Player Settings")]
    [SerializeField] private bool isPlayerOne = true; // Player 1 or Player 2 [true = Player 1, false = Player 2

    private PlayerControls controls;



    private Rigidbody2D rb;
    private Collider2D playerCollider;

    private Collider2D currentSurface;
    private Vector2 moveInput;
    private bool isGrounded;

    private void Awake()
    {
        // Initialize input controls
        controls = new PlayerControls();

        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();

        // Disable default gravity
        rb.gravityScale = 0;

        // Setup input callbacks
        if (isPlayerOne)
        {
            controls.Player1.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
            controls.Player1.Move.canceled += ctx => moveInput = Vector2.zero;
            controls.Player1.Jump.performed += ctx => Jump();
        }
        else
        {
            controls.Player2.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
            controls.Player2.Move.canceled += ctx => moveInput = Vector2.zero;
            controls.Player2.Jump.performed += ctx => Jump();
        }

    }

    private void OnEnable()
    {
        if (isPlayerOne)
            controls.Player1.Enable();
        else
            controls.Player2.Enable();
    }

    private void OnDisable()
    {
        if (isPlayerOne)
            controls.Player1.Disable();
        else
            controls.Player2.Disable();
    }

    private void FixedUpdate()
    {
        FindNearestSurface();
        HandleGravity();
        HandleMovement();
        CheckGrounded();
    }

    private void FindNearestSurface()
    {
        Collider2D[] nearbyColliders = Physics2D.OverlapCircleAll(transform.position, gravityCheckRadius, surfaceLayers);

        if (nearbyColliders.Length > 0)
        {
            float minDistance = float.MaxValue;
            Collider2D nearestSurface = null;

            foreach (Collider2D collider in nearbyColliders)
            {
                Vector2 closestPoint = collider.ClosestPoint(transform.position);
                float distance = Vector2.Distance(transform.position, closestPoint); // Use closest point distance
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestSurface = collider;
                }
            }

            currentSurface = nearestSurface;
        }
        else
        {
            currentSurface = null;
        }
    }

    private void HandleGravity()
    {
        if (currentSurface != null)
        {
            // Calculate direction toward the current surface
            Vector2 closestPoint = currentSurface.ClosestPoint(transform.position);
            Vector2 surfaceDirection = (closestPoint - (Vector2)transform.position).normalized;

            // Apply gravity in the direction of the surface
            rb.AddForce(surfaceDirection * gravityStrength);

            // Adjust player rotation to align with the surface
            Vector2 surfaceUp = currentSurface.transform.up;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.FromToRotation(Vector2.up, surfaceDirection), Time.fixedDeltaTime * 10f);
        }
        else
        {
            rb.AddForce(Vector2.down * gravityStrength);
        }
    }

    private void HandleMovement()
    {
        if (currentSurface != null)
        {
            // Determine the rightward direction relative to the surface
            Vector2 closestPoint = currentSurface.ClosestPoint(transform.position);
            Vector2 surfaceDirection = (closestPoint - (Vector2)transform.position).normalized;
            Vector2 surfaceRight = new Vector2(-surfaceDirection.y, surfaceDirection.x); // Perpendicular to surface direction

            // Move the player along the surface
            Vector2 movementVector = surfaceRight * moveInput.x * moveSpeed;
            rb.linearVelocity = movementVector + Vector2.Dot(rb.linearVelocity, surfaceDirection) * surfaceDirection; // Combine with any residual surface motion
        }
    }

    private void Jump()
    {
        if (isGrounded)
        {
            Vector2 closestPoint = currentSurface.ClosestPoint(transform.position);
            Vector2 jumpDirection = (transform.position - (Vector3)closestPoint).normalized;

            rb.AddForce(jumpDirection * jumpForce, ForceMode2D.Impulse);
        }
    }

    private void CheckGrounded()
    {
        // Simple ground check using the current surface detection
        isGrounded = currentSurface != null &&
            Vector2.Distance(transform.position, currentSurface.ClosestPoint(transform.position)) <= 0.1f + playerCollider.bounds.size.y / 2;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, gravityCheckRadius);
    }
}