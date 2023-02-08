using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InlaidDiamondRingSpawner : BulletSpawner
{
    public DiamondLaser Laser;
    protected override void ShootBullet(Bullet bullet, Transform parent, string name)
    {
        base.ShootBullet(bullet, parent, name);
        bullet.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
    }
    protected override void Evolved()
    {
        base.Evolved();
        if (!GameManager.Inst.player.itemEvos["BloodyDiamond"])
        {
            GameManager.Inst.player.Items["BloodyDiamond"].Evo = true;
        }
    }
}
