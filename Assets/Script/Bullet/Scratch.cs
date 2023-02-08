using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scratch : Bullet
{
    protected override void HitEnemy(Enemy enemy)
    {
        base.HitEnemy(enemy);
        if(entity.Evo)
        {
            enemy.GetCC("Scratch", "bleed", ItemManager.ConvertJToken<float>(entity.data.value["val2"])[1], ItemManager.ConvertJToken<float>(entity.data.value["val2"])[0]);
        }
    }
}
