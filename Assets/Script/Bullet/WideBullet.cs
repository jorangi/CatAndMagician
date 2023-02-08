using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WideBullet : Bullet
{
    protected override void HitEnemy()
    {
        for (int i = 0; i < HitTarget.Count; i++)
        {
            Knockback(HitTarget[i]);
            HitEnemy(HitTarget[i]);
        }
        HitTarget.Clear();
    }
}
