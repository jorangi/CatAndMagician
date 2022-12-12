using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottleCap : MonoBehaviour
{
    private float speed = 7;
    private void Update()
    {
        transform.Translate(speed * Time.deltaTime * Vector2.down);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !GameManager.Inst.player.Invincible)
        {
            GameManager.Inst.player.HP -= 4;
            Destroy(gameObject);
        }
        if(collision.CompareTag("Remove"))
        {
            Destroy(gameObject);
        }
    }
}
