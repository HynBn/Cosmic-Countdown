using UnityEngine;

public class ItemSpeedBoost : BaseItem
{
    protected override void ApplyEffect(Player player)
    {
        player.Modifier.ModifyMoveSpeed(1.5f, 3.0f);
    }
}
