using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnifyingGlass : Item
{
    protected override void LevelChanged()
    {
        base.LevelChanged();
        GameManager.Inst.player.BulletSize *= 1 + ItemManager.ConvertJToken<float>(data.value["val0"])[Mathf.Min(ItemManager.ConvertJToken<float>(data.value["val0"]).Length - 1, Lv - 1)] * 0.01f;
    }
    protected override void Evolved()
    {
        base.Evolved();
        float s = ItemManager.ConvertJToken<float>(data.value["val1"])[0];
        GameManager.Inst.player.transform.localScale = new(s, s);
        foreach(Transform tr in GameManager.Inst.player.transform)
        {
            tr.localScale = new(1 / s, 1 / s);
        }
    }
}
