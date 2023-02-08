using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CatsEye : Bullet
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.CompareTag("Enemy"))
        {
            if(entity.Evo)
            {
                Transform obj = GameObject.FindGameObjectWithTag("Bullets").transform.Find("CatsEyeExplosion").GetChild(0);
                obj.gameObject.SetActive(true);
                obj.SetParent(null);
                obj.transform.position = transform.position;
                CatsEyeExplosion ex = obj.GetComponent<CatsEyeExplosion>();
                ex.dmg = dmg;
            }
        }
    }
}
