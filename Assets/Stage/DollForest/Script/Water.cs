using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    public Enemy parent;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !GameManager.Inst.player.Invincible)
        {
            GameManager.Inst.player.Hit(parent, 4);
        }
    }
}
