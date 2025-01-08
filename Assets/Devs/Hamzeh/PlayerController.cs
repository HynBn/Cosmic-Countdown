using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Player player;

    [Header("Surface Detection")]
    [SerializeField] private float gravityCheckRadius = 5f;
    [SerializeField] private LayerMask surfaceLayers;

    [Header("Player Settings")]
    [SerializeField] private bool isPlayerOne = true; // Player 1 or Player 2 [true = Player 1, false = Player 2

    private PlayerControls controls;

    private Rigidbody2D rb;
    private Collider2D playerCollider;

    private Collider2D currentSurface;
    private Vector2 moveInput;

    private void Awake()
    {
        player = GetComponent<Player>();

        // Initialize input controls
        controls = new PlayerControls();

        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();

       

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
    }


    /*
    FindNearestSurface checks an area around the player for surfaces and stores the nearest one in currentSurface.
    */
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
    }

    private void HandleGravity()
    {
        if (currentSurface != null)
        {
            // Calculate direction toward the current surface
            Vector2 closestPoint = currentSurface.ClosestPoint(transform.position);
            Vector2 surfaceDirection = (closestPoint - (Vector2)transform.position).normalized;

            // Apply gravity in the direction of the surface
            rb.AddForce(surfaceDirection * player.GravityStrength);

            // Adjust player rotation to align with the surface
            // TODO: adjust rotation based on player sprites
            // Calculate the target angle based on the surface direction
            float targetAngle = Mathf.Atan2(surfaceDirection.y, surfaceDirection.x) * Mathf.Rad2Deg - 90f;

            // Flip the player 180 degrees
            targetAngle += 180f;

            // Smoothly interpolate the rotation from the current angle to the target angle
            float smoothAngle = Mathf.LerpAngle(transform.eulerAngles.z, targetAngle, Time.fixedDeltaTime * 10f);

            // Apply the smoothed rotation
            transform.rotation = Quaternion.Euler(0f, 0f, smoothAngle);

        }
        else
        {
            rb.AddForce(Vector2.down * player.GravityStrength);
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
            Vector2 movementVector = surfaceRight * moveInput.x * player.MoveSpeed * player.Direction;
            rb.linearVelocity = movementVector + Vector2.Dot(rb.linearVelocity, surfaceDirection) * surfaceDirection; // Combine with any residual surface motion
        }
    }

    private void Jump()
    {
        if (IsGrounded())
        {
            Vector2 closestPoint = currentSurface.ClosestPoint(transform.position);
            Vector2 jumpDirection = (transform.position - (Vector3)closestPoint).normalized;

            rb.AddForce(jumpDirection * player.JumpForce, ForceMode2D.Impulse);
        }
    }

    private bool IsGrounded()
    {
        // Simple ground check using the current surface detection
        return currentSurface != null && Vector2.Distance(transform.position, currentSurface.ClosestPoint(transform.position)) <= 0.3f + playerCollider.bounds.size.y / 2;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, gravityCheckRadius);
    }
}