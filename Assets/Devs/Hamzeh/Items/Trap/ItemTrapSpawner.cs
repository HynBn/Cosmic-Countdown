using UnityEngine;

public class ItemTrapSpawner : BaseItem
{
    [SerializeField] private GameObject trapPrefab;
    protected override void ApplyEffect(Player player)
    {
        itemManager.SpawnTrapAfterDelay(trapPrefab, transform.position, 0.3f);
    }
}
