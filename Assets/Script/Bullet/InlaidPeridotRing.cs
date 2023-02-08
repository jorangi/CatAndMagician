using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InlaidPeridotRing : InlaidProjectile
{
    protected override void HitEnemy(float dmg, Enemy enemy)
    {
       foreach(CC cC in enemy.CC)
        {
            if(cC.type == "poison")
            {
                base.HitEnemy(dmg * (1 + ItemManager.ConvertJToken<float>(entity.data.value["val1"])[0] * 0.01f), enemy);
                return;
            }
        }
        base.HitEnemy(dmg, enemy);
    }
}
