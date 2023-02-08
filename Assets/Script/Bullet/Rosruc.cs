using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rosruc : Bullet
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.CompareTag("Enemy"))
        {
            (entity as RosrucSpawner).HitCount++;
            (entity as RosrucSpawner).HittedEnemy = collision.transform;
        }
    }
}
