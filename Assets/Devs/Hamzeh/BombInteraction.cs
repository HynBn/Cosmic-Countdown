using UnityEngine;

public class BombInteraction : MonoBehaviour
{
    private SpriteRenderer bombSprite;
    private PlayerAttributes playerAttributes;
    private bool canTransferBomb = true;
    private float transferCooldown = 0.5f;

    private void Awake()
    {
        bombSprite = GetComponent<SpriteRenderer>();
        playerAttributes = transform.parent.GetComponent<PlayerAttributes>();

        if (bombSprite == null)
        {
            Debug.LogError($"No SpriteRenderer found on {gameObject.name}!");
        }
        if (playerAttributes == null)
        {
            Debug.LogError($"No PlayerAttributes found on parent of {gameObject.name}!");
        }
    }

    private void Start()
    {
        playerAttributes.OnBombStatusChanged += HandleBombStatusChanged;
        bombSprite.enabled = playerAttributes.HasBomb;
    }

    private void OnDestroy()
    {
        if (playerAttributes != null)
        {
            playerAttributes.OnBombStatusChanged -= HandleBombStatusChanged;
        }
    }

    private void HandleBombStatusChanged(bool hasBomb)
    {
        bombSprite.enabled = hasBomb;
    }

    public void SetBombStatus(bool hasBomb)
    {
        playerAttributes.HasBomb = hasBomb;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Only handle collision if we have the bomb and can transfer
        if (!playerAttributes.HasBomb || !canTransferBomb) return;

        BombInteraction otherBomb = collision.GetComponent<BombInteraction>();
        if (otherBomb != null && otherBomb.canTransferBomb)
        {
            // Prevent both objects from handling the same collision
            canTransferBomb = false;
            otherBomb.canTransferBomb = false;

            SetBombStatus(false);
            otherBomb.SetBombStatus(true);

            // Reset transfer ability after cooldown for both objects
            Invoke(nameof(ResetTransferAbility), transferCooldown);
            otherBomb.Invoke(nameof(ResetTransferAbility), transferCooldown);
        }
    }

    private void ResetTransferAbility()
    {
        canTransferBomb = true;
    }
}