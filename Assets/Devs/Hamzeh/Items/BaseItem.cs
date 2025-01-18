using UnityEngine;

public abstract class BaseItem : MonoBehaviour
{
    protected ItemManager itemManager;
    protected Transform spawnPoint;

    public void Initialize(ItemManager manager, Transform spawn)
    {
        itemManager = manager;
        spawnPoint = spawn;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player != null)
        {
            HandleCollection(player);
            Destroy(gameObject);
        }
    }

    protected virtual void HandleCollection(Player player)
    {
        ReturnSpawnPoint();
        ApplyEffect(player);
    }

    protected void ReturnSpawnPoint()
    {
        itemManager.OnItemCollected(spawnPoint);
    }

    protected abstract void ApplyEffect(Player player);
}