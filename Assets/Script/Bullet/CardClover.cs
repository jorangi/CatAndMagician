using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardClover : Bullet
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.CompareTag("Enemy"))
        {
            Transform obj = (entity as BulletSpawner).Bullets.transform.Find("CloverExplosion").GetChild(0);

            obj.gameObject.SetActive(true);
            obj.SetParent(null);
            obj.transform.position = transform.position;
            CloverExplosion ex = obj.GetComponent<CloverExplosion>();
            ex.dmg = dmg;
        }
    }
}