using UnityEngine;

public class PlayerAttributes : MonoBehaviour
{
    public float BaseMoveSpeed { get; private set; } = 10f;
    public float BaseJumpForce { get; private set; } = 20f;
    public float BaseGravityStrength { get; private set; } = 100f;
    public int BaseDirection { get; private set; } = 1;

    public float MoveSpeed { get; set; }
    public float JumpForce { get; set; }
    public float GravityStrength { get; set; }
    public int Direction { get; set; }

    // event triggered when the bomb changes and notifies subscribers
    public event System.Action<bool> OnBombStatusChanged;
    private bool _hasBomb;
    public bool HasBomb
    {
        get => _hasBomb;
        set
        {
            _hasBomb = value;
            OnBombStatusChanged?.Invoke(_hasBomb);
        }
    }

    private void Awake()
    {
        ResetToBaseStats();
    }

    private void ResetToBaseStats()
    {
        MoveSpeed = BaseMoveSpeed;
        JumpForce = BaseJumpForce;
        GravityStrength = BaseGravityStrength;
        Direction = BaseDirection;
    }
}