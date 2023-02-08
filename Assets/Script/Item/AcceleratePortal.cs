using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcceleratePortal : Item
{
    protected override void LevelChanged()
    {
        base.LevelChanged();
        float[] value = ConvertJToken<float>(data.value["val0"]);
        GameManager.Inst.player.BulletSpeedRatio *= 1 + value[Mathf.Min(value.Length - 1, Lv - 1)] * 0.01f;
    }
}
