using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicArrowSpawner : BulletSpawner
{
    public int prong;
    protected override void ShootBullet()
    {
        prong = ItemManager.ConvertJToken<int>(data.value["val0"])[Mathf.Min(ItemManager.ConvertJToken<int>(data.value["val0"]).Length - 1, Lv - 1)];

        if (Evo) 
            prong = ItemManager.ConvertJToken<int>(data.value["val1"])[0];

        for (int i = 0; i < prong; i++)
        {
            ShootBullet(Bullets.transform.Find(data.value["id"].ToString()).GetChild(0).GetComponent<Bullet>(), Bullets.transform.Find(data.value["id"].ToString()).GetChild(0), data.value["id"].ToString(), i);
        }
    }
    private void ShootBullet(Bullet bullet, Transform parent, string name, int i)
    {
        base.ShootBullet(bullet, parent, name);
        GameObject obj = bullet.gameObject;
        obj.transform.rotation = Quaternion.Euler(0, 0, -30 + i * 60 / Mathf.Max(prong - 1, 1));
        if (prong == 1)
        {
            obj.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
