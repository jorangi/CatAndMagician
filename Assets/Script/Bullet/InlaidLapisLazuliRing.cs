using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InlaidLapisLazuliRing : InlaidProjectile
{
    protected override void HitEnemy(float dmg, Enemy enemy)
    {
        base.HitEnemy(dmg, enemy);

        if (enemy.HP <= 0)
        {
            InlaidLapisLazuliRingSpawner lapis = (InlaidLapisLazuliRingSpawner)entity;
            lapis.OnAbility = StartCoroutine(lapis.Ability());
        }
    }
}
