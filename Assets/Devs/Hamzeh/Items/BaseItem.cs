using UnityEngine;

public abstract class BaseItem : MonoBehaviour
{
    [SerializeField]
    protected ItemManager itemManager;

    public void Initialize(ItemManager manager)
    {
        itemManager = manager;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (!itemManager.IsGameActive()) return;

        Player player = collision.GetComponent<Player>();
        if (player != null)
        {
            ApplyEffect(player);
            Destroy(gameObject);
        }
    }

    protected abstract void ApplyEffect(Player player);
}