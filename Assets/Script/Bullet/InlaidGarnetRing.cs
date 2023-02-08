using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InlaidGarnetRing : InlaidProjectile
{
    protected override void HitEnemy(float dmg, Enemy enemy)
    {
        dmg *= 1 + ItemManager.ConvertJToken<float>(entity.data.value["val0"])[Mathf.Max(0, Mathf.Min(ItemManager.ConvertJToken<float>(entity.data.value["val0"]).Length - 1, entity.Lv - 1))];
        base.HitEnemy(dmg, enemy);
    }
}
