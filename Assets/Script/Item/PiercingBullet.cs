using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiercingBullet : Item
{
    protected override void LevelChanged()
    {
        base.LevelChanged();
        GameManager.Inst.player.PiercingTarget = ItemManager.ConvertJToken<int>(data.value["val0"])[Mathf.Min(ItemManager.ConvertJToken<int>(data.value["val0"]).Length - 1, Lv - 1)];
    }
}
