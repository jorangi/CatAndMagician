using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueExpStar : Item
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.Inst.player.Exp += 3;
        }
        base.OnTriggerEnter2D(collision);
    }
}
