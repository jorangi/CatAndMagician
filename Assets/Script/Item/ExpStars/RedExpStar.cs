using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedExpStar : Item
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.Inst.player.Exp += 10;
        }
        base.OnTriggerEnter2D(collision);
    }
}
