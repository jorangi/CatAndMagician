using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiamondBullet : InlaidProjectile
{
    public bool isPoint;
    public InlaidDiamondRing parent;
    public Bullet sibling;
    protected override void FixedUpdate()
    {
    }
    protected override void HitEnemy()
    {
        for (int i = 0; i < HitTarget.Count; i++)
        {
            if (sibling.HitTarget.Contains(HitTarget[i]))
            {
                continue;
            }
            Knockback(HitTarget[i]);
            HitEnemy(HitTarget[i]);
        }
        IncreaseInlaidProjectileStack();
    }
    protected override void HitEnemy(float dmg, Enemy enemy)
    {
        if (isPoint)
        {
            dmg = parent.dmg * (1 + ItemManager.ConvertJToken<float>(parent.entity.data.value["val0"])[0] * 0.01f);
        }
        else
            dmg = parent.dmg;
        base.HitEnemy(dmg, enemy);
    }
    protected override void PierceCheck(bool nonPierce)
    {
        if (parent.MaxPierce > 0)
        {
            parent.HitCount++;
            if (GameManager.Inst.player.Items["PiercingBullet"].Evo)
            {
                parent.AddPiercingDmg();
            }
            parent.MaxPierce--;
        }
        else if (!nonPierce)
        {
            parent.ReturnObject();
        }
    }
    protected override IEnumerator ReturnObj()
    {
        parent.ReturnObject();
        yield break;
    }
}
