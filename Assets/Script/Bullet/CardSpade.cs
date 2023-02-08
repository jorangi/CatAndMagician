using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSpade : Bullet
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.CompareTag("Enemy"))
        {
            collision.transform.parent.GetComponent<Enemy>().GetCC("CardSpade", "vulnerable", ItemManager.ConvertJToken<float>(ItemManager.datas["BrokenWatch"].value["val0"])[4] * 0.01f, ItemManager.ConvertJToken<float>(ItemManager.datas["BrokenWatch"].value["val0"])[3]);
        }
    }
}
