using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrozenNametag : Item
{
    protected override void LevelChanged()
    {
        base.LevelChanged();
        GameManager.Inst.player.BulletSpeedRatio /= 1 + ItemManager.ConvertJToken<float>(data.value["val0"])[Mathf.Min(ItemManager.ConvertJToken<float>(data.value["val0"]).Length - 1, Lv - 1)] * 0.01f;
    }
}
