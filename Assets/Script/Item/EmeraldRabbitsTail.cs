using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmeraldRabbitsTail : Item
{
    protected override void LevelChanged()
    {
        base.LevelChanged();
        GameManager.Inst.player.Knockback += ItemManager.ConvertJToken<float>(data.value["val0"])[Mathf.Min(ItemManager.ConvertJToken<float>(data.value["val0"]).Length - 1, Lv - 1)];
    }
}
