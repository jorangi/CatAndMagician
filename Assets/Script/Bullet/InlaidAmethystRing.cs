using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InlaidAmethystRing : InlaidProjectile
{
    protected override void HitEnemy(float dmg, Enemy enemy)
    {
        base.HitEnemy(dmg, enemy);
        float[] t = ItemManager.ConvertJToken<float>(entity.data.value["val0"]);
        enemy.GetCC("InlaidAmethystRing", "slow", t[1] * 0.01f, t[0]);
        if (enemy.HP <= 0)
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }
    }
}
