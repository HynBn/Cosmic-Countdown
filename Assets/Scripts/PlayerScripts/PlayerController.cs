using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Player player;

    [Header("Surface Detection")]
    [SerializeField] private float gravityCheckRadius = 5f;
    [SerializeField] private LayerMask surfaceLayers;
    [SerializeField] private GameManager gameManager;

    [Header("Player Settings")]
    [SerializeField] private bool isPlayerOne = true; // Player 1 or Player 2 [true = Player 1, false = Player 2]

    [Header("Animation")]
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private readonly string IS_MOVING = "IsMoving";
    private readonly string IS_JUMPING = "IsJumping";
    private readonly string TRIGGER_WIN = "TriggerWin";
    private readonly string TRIGGER_EXPLODE = "TriggerExplode";
    private readonly string TRIGGER_RESET = "TriggerReset";

    private PlayerControls controls;
    private Rigidbody2D rb;
    private Collider2D playerCollider;
    private Collider2D currentSurface;
    private Vector2 moveInput;

    // Cached surface calculations
    private Vector2 surfaceDirection;
    private Vector2 surfaceRight;

    private void Awake()
    {
        // Get core components
        player = GetComponent<Player>();
        if (player == null)
            Debug.LogError($"No Player component found on {gameObject.name}!");

        // Get physics components
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();

        // Get visual components
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (animator == null)
            Debug.LogError($"Missing Animator component on {gameObject.name}!");
        if (spriteRenderer == null)
            Debug.LogError($"Missing SpriteRenderer component on {gameObject.name}!");

        // Initialize input controls
        controls = new PlayerControls();

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
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
    }

    private void FixedUpdate()
    {
        if (gameManager.IsGameActive())
        {


            FindNearestSurface();
            CalculateSurfaceDirections();
            HandleGravity();
            HandleMovement();
            UpdateMovementAnimation();

            // Update jumping animation based on grounded state
            animator.SetBool(IS_JUMPING, !IsGrounded());
        }
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
                float distance = Vector2.Distance(transform.position, closestPoint);
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

    private void CalculateSurfaceDirections()
    {
        if (currentSurface != null)
        {
            Vector2 closestPoint = currentSurface.ClosestPoint(transform.position);
            surfaceDirection = (closestPoint - (Vector2)transform.position).normalized;
            surfaceRight = new Vector2(-surfaceDirection.y, surfaceDirection.x);
        }
    }

    private void HandleGravity()
    {
        if (currentSurface != null)
        {
            // Apply gravity in the direction of the surface
            rb.AddForce(surfaceDirection * player.Attributes.GravityStrength);

            // Calculate the target angle based on the surface direction
            float targetAngle = Mathf.Atan2(surfaceDirection.y, surfaceDirection.x) * Mathf.Rad2Deg - 90f;
            targetAngle += 180f;

            // Smoothly interpolate the rotation
            float smoothAngle = Mathf.LerpAngle(transform.eulerAngles.z, targetAngle, Time.fixedDeltaTime * 10f);

            // Apply the smoothed rotation
            transform.rotation = Quaternion.Euler(0f, 0f, smoothAngle);
        }
        else
        {
            rb.AddForce(Vector2.down * player.Attributes.GravityStrength);
        }
    }

    private void HandleMovement()
    {
        if (currentSurface != null)
        {
            // Use precalculated directions for movement
            Vector2 movementVector = surfaceRight * moveInput.x * player.Attributes.MoveSpeed * player.Attributes.Direction;
            rb.linearVelocity = movementVector + Vector2.Dot(rb.linearVelocity, surfaceDirection) * surfaceDirection;
        }
    }

    private void Jump()
    {
        if (IsGrounded())
        {
            Vector2 jumpDirection = -surfaceDirection;  // Using precalculated surface direction
            rb.AddForce(jumpDirection * player.Attributes.JumpForce, ForceMode2D.Impulse);
            animator.SetBool(IS_JUMPING, true);
        }
    }

    private bool IsGrounded()
    {
        return currentSurface != null && Vector2.Distance(transform.position, currentSurface.ClosestPoint(transform.position)) <= 0.5f + playerCollider.bounds.size.y / 2;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, gravityCheckRadius);
    }

    // Animation ---------------------------------------------------------------

    private void UpdateMovementAnimation()
    {
        bool isMoving = Mathf.Abs(moveInput.x) > 0.1f;
        animator.SetBool(IS_MOVING, isMoving);

        if (isMoving && currentSurface != null)
        {
            // For wall movement, we just need to know if we're moving with or against gravity
            bool movingUp = moveInput.x > 0;
            spriteRenderer.flipX = movingUp;
        }
    }

    public void TriggerWinAnimation()
    {
        // Clear any existing triggers first
        animator.ResetTrigger(TRIGGER_RESET);
        animator.ResetTrigger(TRIGGER_EXPLODE);
        animator.SetTrigger(TRIGGER_WIN);
    }

    public void TriggerExplosionAnimation()
    {
        player.Attributes.HasBomb = false;
        // Clear any existing triggers first
        animator.ResetTrigger(TRIGGER_RESET);
        animator.ResetTrigger(TRIGGER_WIN);
        animator.SetTrigger(TRIGGER_EXPLODE);
    }

    public void ResetToIdle()
    {
        // Clear other triggers first
        animator.ResetTrigger(TRIGGER_WIN);
        animator.ResetTrigger(TRIGGER_EXPLODE);
        animator.SetTrigger(TRIGGER_RESET);
        animator.SetBool(IS_JUMPING, false);
        animator.SetBool(IS_MOVING, false);
    }


}