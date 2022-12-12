using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBallSpawner : BulletSpawner
{
    public int bounce = 0;

    protected override void LevelChanged()
    {
        bounce = Lv;
    }
    protected override void Evolved()
    {

    }
    protected override void ShootBullet()
    {
        GameObject obj = Bullets.transform.Find(bulletData.name).GetChild(0).gameObject;
        MagicBall bullet = obj.GetComponent<MagicBall>();
        bullet.bounce = bounce;
        base.ShootBullet();
    }
}
