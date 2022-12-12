using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowExpStar : Item
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.Inst.player.Exp += 1;
        }
        base.OnTriggerEnter2D(collision);
    }
}
