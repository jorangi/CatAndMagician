using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : Item
{
    protected override void LevelChanged()
    {
        base.LevelChanged();
        GameManager.Inst.player.Knockback += ItemManager.ConvertJToken<float>(data.value["val0"])[Mathf.Min(ItemManager.ConvertJToken<float>(data.value["val0"]).Length, Lv - 1)];
    }
}
