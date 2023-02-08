using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InlaidTopazRing : InlaidProjectile
{
    protected override void HitEnemy(float dmg, Enemy enemy)
    {
        foreach(CC cC in enemy.CC)
        {
            if(cC.type == "bleed")
            {
                base.HitEnemy(dmg * (1 + ItemManager.ConvertJToken<float>((entity as InlaidTopazRingSpawner).data.value["val0"])[0] * 0.01f), enemy);
                if(enemy.HP <= 0)
                {
                    (GameManager.Inst.player.Items["FragmentOfMonster"] as FragmentOfMonster).Scratch();
                }
                return;
            }
        }
        base.HitEnemy(dmg, enemy);
    }
}
