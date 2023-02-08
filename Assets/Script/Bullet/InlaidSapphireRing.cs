using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InlaidSapphireRing : InlaidProjectile
{
    protected override void HitEnemy(float dmg, Enemy enemy)
    {
        base.HitEnemy(dmg, enemy);
        if(Random.Range(0f, 1f) <= ItemManager.ConvertJToken<float>(entity.data.value["val0"])[0] *0.01f)
        {
            GameObject obj = Instantiate((entity as InlaidSapphireRingSpawner).SapphireWall);
            obj.transform.position = new(enemy.transform.position.x, enemy.transform.position.y - 2);
        }
    }
}
