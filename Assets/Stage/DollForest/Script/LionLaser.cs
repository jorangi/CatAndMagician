using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LionLaser : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !GameManager.Inst.player.Invincible)
        {
            GameManager.Inst.player.HP -= 10;
        }
    }
}
