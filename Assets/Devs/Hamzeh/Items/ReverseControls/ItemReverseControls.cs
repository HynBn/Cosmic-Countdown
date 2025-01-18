using UnityEngine;

public class ItemReverseControls : BaseItem
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void ApplyEffect(Player player)
    {
        player.Modifier.ReverseControls(3.0f);
        itemManager.GetOtherPlayer(player).Modifier.ReverseControls(3.0f);
    }
}
