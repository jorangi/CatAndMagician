using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicArrowSpawner : BulletSpawner
{
    public int prong;
    protected override void ShootBullet()
    {
        switch(Lv)
        {
            case 1:
                prong = 1;
                break;
            case 2:
                prong = 1;
                break;
            case 3:
                prong = 3;
                break;
            case 4:
                prong = 3;
                break;
            case 5:
                prong = 5;
                break;
        }
        for(int i = 0; i < prong; i++)
        {
            GameObject obj = Bullets.transform.Find(bulletData.name).GetChild(0).gameObject;
            obj.transform.SetParent(null);
            Bullet bullet = obj.GetComponent<Bullet>();
            bullet.dmg = dmg * GameManager.Inst.player.BulletDmg;
            bullet.speed = speed * GameManager.Inst.player.BulletSpeed;
            bullet.knockback = bulletData.knockback * GameManager.Inst.player.Knockback;
            obj.transform.localScale = new(GameManager.Inst.player.BulletSize, GameManager.Inst.player.BulletSize);
            obj.transform.rotation = Quaternion.Euler(0, 0, -30 + i * 60 / Mathf.Max(prong - 1, 1));
            if (prong == 1)
            {
                obj.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            obj.name = bulletData.name;
            obj.transform.position = transform.position;
            obj.SetActive(true);
        }
    }
}
