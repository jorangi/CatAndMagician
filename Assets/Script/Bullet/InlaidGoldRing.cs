using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InlaidGoldRing : InlaidProjectile
{
    protected override void HitEnemy(Enemy enemy)
    {
        base.HitEnemy(enemy);
        if (entity.Evo && Random.Range(0f, 1f) <= ItemManager.ConvertJToken<float>(entity.data.value["val0"])[0])
        {
            (GameManager.Inst.player.Items["GoldCube"] as GoldCube).Gold++;
        }
    }
}
