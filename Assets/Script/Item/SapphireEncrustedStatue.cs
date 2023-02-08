using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SapphireEncrustedStatue : Item
{
    public GameObject sapphireObj;
    protected override void LevelChanged()
    {
        base.LevelChanged();
        GameManager.Inst.player.invincibleTime += ItemManager.ConvertJToken<float>(data.value["val0"])[Mathf.Min(ItemManager.ConvertJToken<float>(data.value["val0"]).Length - 1, Lv - 1)];
    }
}
