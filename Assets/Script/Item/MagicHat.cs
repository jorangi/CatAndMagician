using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicHat : Item
{
    protected override void LevelChanged()
    {
        base.LevelChanged();
        GameManager.Inst.player.BulletDmgRatio *= 1 + ItemManager.ConvertJToken<float>(data.value["val0"])[Mathf.Min(ItemManager.ConvertJToken<float>(data.value["val0"]).Length - 1, Lv - 1)] * 0.01f;
    }
    protected override void Evolved()
    {
        GameManager.Inst.player.BulletDmgRatio *= 1 + ItemManager.ConvertJToken<float>(data.value["val1"])[0] * 0.01f;
    }
}
