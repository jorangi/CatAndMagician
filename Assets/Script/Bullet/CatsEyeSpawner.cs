using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatsEyeSpawner : BulletSpawner
{
    public GameObject Explosion;
    protected override void PoolingBullet()
    {
        base.PoolingBullet(); 

        if (Bullets.transform.Find("CatsEyeExplosion"))
        {
            return;
        }
        GameObject parent = new();
        parent.transform.SetParent(Bullets.transform);
        parent.name = "CatsEyeExplosion";

        for (int i = 0; i < (int)data.value["limit"]; i++)
        {
            GameObject obj = Instantiate(Explosion, parent.transform);
            obj.name = "CatsEyeExplosion";
            obj.GetComponent<CatsEyeExplosion>().entity = this;
            obj.SetActive(false);
        }
    }
}
