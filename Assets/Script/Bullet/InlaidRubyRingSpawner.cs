using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InlaidRubyRingSpawner : BulletSpawner
{
    protected override void ShootBullet(Bullet bullet, Transform parent, string name)
    {
        base.ShootBullet(bullet, parent, name);
        if (Evo && GameManager.Inst.player.HP >= GameManager.Inst.player.maxhp)
        {
            bullet.dmg = dmg * GameManager.Inst.player.BulletDmgRatio + GameManager.Inst.player.NumericalBulletDmg * ItemManager.ConvertJToken<float>(data.value["val0"])[0] * 0.01f;
        }
        else
        {
            bullet.dmg = dmg * GameManager.Inst.player.BulletDmgRatio + GameManager.Inst.player.NumericalBulletDmg;
        }
    }
    protected override void Evolved()
    {
        base.Evolved();
        if (!GameManager.Inst.player.itemEvos["BigRuby"])
        {
            GameManager.Inst.player.Items["BigRuby"].Evo = true;
        }
    }
}
