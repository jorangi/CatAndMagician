using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InlaidProjectile : Bullet
{
    protected override void HitEnemy()
    {
        base.HitEnemy();
        IncreaseInlaidProjectileStack();
    }
    protected void IncreaseInlaidProjectileStack()
    {
        GameManager.Inst.player.HitInlaidProjectile++;
    }
}
