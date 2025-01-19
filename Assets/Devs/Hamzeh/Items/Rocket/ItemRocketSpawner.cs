using UnityEngine;

public class ItemRocketSpawner : BaseItem
{
    [SerializeField] private GameObject rocketPrefab;

    protected override void ApplyEffect(Player player)
    {
        // Get the target player (the other player)
        Player targetPlayer = itemManager.GetOtherPlayer(player);

        // Spawn the rocket and set its target
        GameObject rocket = Instantiate(rocketPrefab, transform.position, Quaternion.identity);
        ItemRocket rocketComponent = rocket.GetComponent<ItemRocket>();
        if (rocketComponent != null)
        {
            rocketComponent.SetTarget(targetPlayer);
        }
    }
}
