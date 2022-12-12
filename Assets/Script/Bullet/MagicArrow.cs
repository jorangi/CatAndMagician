using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicArrow : Bullet
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.CompareTag("Remove"))
        {
            ReturnObject();
        }
        else if (collision.CompareTag("Enemy"))
        {
            if (collision.GetComponent<Enemy>() != null)
            {
                collision.transform.Translate(Vector2.up * knockback);
                collision.GetComponent<Enemy>().HP -= dmg;
            }
            else if (collision.GetComponent<Boss>() != null)
            {
                collision.GetComponent<Boss>().HP -= dmg;
            }
            ReturnObject();
        }
    }
}
