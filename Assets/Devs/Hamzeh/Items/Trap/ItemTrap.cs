using UnityEngine;
using System.Collections;

public class ItemTrap : BaseItem
{
    protected override void ApplyEffect(Player player)
    {
        player.Modifier.ModifyMoveSpeed(0.0f, 2.0f);
    }
}
