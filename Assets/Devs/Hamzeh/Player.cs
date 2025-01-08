using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float baseMoveSpeed = 10f;
    [SerializeField] private float baseJumpForce = 20f;
    [SerializeField] private float baseGravityStrength = 100f;
    private int baseDirection = 1;

    // Current modified values
    private float currentMoveSpeed;
    private float currentJumpForce;
    private float currentGravityStrength;
    private int currentDirection;

    // Public properties to access current values
    public float MoveSpeed => currentMoveSpeed;
    public float JumpForce => currentJumpForce;
    public float GravityStrength => currentGravityStrength;
    public int Direction => currentDirection;

    private void Awake()
    {
        // Initialize current values with base values
        ResetToBaseStats();
    }

    public void ResetToBaseStats()
    {
        currentMoveSpeed = baseMoveSpeed;
        currentJumpForce = baseJumpForce;
        currentGravityStrength = baseGravityStrength;
        currentDirection = baseDirection;
    }

    // Methods for modifying stats
    
}