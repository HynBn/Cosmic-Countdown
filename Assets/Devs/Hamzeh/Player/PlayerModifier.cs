using UnityEngine;
using System.Collections;

public class PlayerModifier : MonoBehaviour
{
    private Player player;

    private void Awake()
    {
        player = GetComponent<Player>();
        if (player == null)
        {
            Debug.LogError($"No Player component found on {gameObject.name}!");
        }
    }

    public void ResetAllAttributes()
    {
        player.Attributes.MoveSpeed = player.Attributes.BaseMoveSpeed;

        player.Attributes.JumpForce = player.Attributes.BaseJumpForce;

        player.Attributes.GravityStrength = player.Attributes.BaseGravityStrength;

        player.Attributes.Direction = player.Attributes.BaseDirection;
    }

    public void ModifyMoveSpeed(float speedMultiplier, float duration = 0f)
    {
        if (duration > 0)
        {
            StartCoroutine(TemporarySpeedModification(speedMultiplier, duration));
        }
        else
        {
            player.Attributes.MoveSpeed = player.Attributes.BaseMoveSpeed * speedMultiplier;
        }
    }

    public void ModifyJumpForce(float jumpMultiplier, float duration = 0f)
    {
        if (duration > 0)
        {
            StartCoroutine(TemporaryJumpModification(jumpMultiplier, duration));
        }
        else
        {
            player.Attributes.JumpForce = player.Attributes.BaseJumpForce * jumpMultiplier;
        }
    }

    public void ModifyGravity(float gravityMultiplier, float duration = 0f)
    {
        if (duration > 0)
        {
            StartCoroutine(TemporaryGravityModification(gravityMultiplier, duration));
        }
        else
        {
            player.Attributes.GravityStrength = player.Attributes.BaseGravityStrength * gravityMultiplier;
        }
    }

    public void ReverseControls(float duration)
    {
        StartCoroutine(TemporaryControlReversal(duration));
    }

    private IEnumerator TemporarySpeedModification(float speedMultiplier, float duration)
    {
        float originalSpeed = player.Attributes.MoveSpeed;
        player.Attributes.MoveSpeed = player.Attributes.BaseMoveSpeed * speedMultiplier;

        yield return new WaitForSeconds(duration);

        player.Attributes.MoveSpeed = originalSpeed;
    }

    private IEnumerator TemporaryJumpModification(float jumpMultiplier, float duration)
    {
        float originalJumpForce = player.Attributes.JumpForce;
        player.Attributes.JumpForce = player.Attributes.BaseJumpForce * jumpMultiplier;

        yield return new WaitForSeconds(duration);

        player.Attributes.JumpForce = originalJumpForce;
    }

    private IEnumerator TemporaryGravityModification(float gravityMultiplier, float duration)
    {
        float originalGravity = player.Attributes.GravityStrength;
        player.Attributes.GravityStrength = player.Attributes.BaseGravityStrength * gravityMultiplier;

        yield return new WaitForSeconds(duration);

        player.Attributes.GravityStrength = originalGravity;
    }

    private IEnumerator TemporaryControlReversal(float duration)
    {
        player.Attributes.Direction *= -1;

        yield return new WaitForSeconds(duration);

        player.Attributes.Direction *= -1;
    }
}