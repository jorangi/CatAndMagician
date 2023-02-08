using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDiamond : Bullet
{
    public GameObject fragmentDiamond;
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.CompareTag("Enemy"))
        {
            GameObject obj = Instantiate(fragmentDiamond);
            obj.transform.position = transform.position;
        }
    }
}