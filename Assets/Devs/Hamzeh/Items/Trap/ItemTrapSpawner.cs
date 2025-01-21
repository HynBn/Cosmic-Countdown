using UnityEngine;

public class ItemTrapSpawner : BaseItem
{
    [SerializeField] private GameObject trapPrefab;

    protected override void HandleCollection(Player player)
    {
        // Don't return the spawn point, just apply effect
        ApplyEffect(player);
    }

    protected override void ApplyEffect(Player player)
    {
        itemManager.SpawnTrapAfterDelay(trapPrefab, transform.position, 0.8f, spawnPoint);
    }
}