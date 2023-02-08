using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Talisman : Item
{
    protected override void LevelChanged()
    {
        base.LevelChanged();
        GameManager.Inst.player.Talisman = ItemManager.ConvertJToken<float>(data.value["val0"])[Mathf.Min(ItemManager.ConvertJToken<float>(data.value["val0"]).Length - 1, Lv - 1)];
        GameManager.Inst.player.OnTalisman = true;
    }
}
