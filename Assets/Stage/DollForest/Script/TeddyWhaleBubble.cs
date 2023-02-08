using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeddyWhaleBubble : MonoBehaviour
{
    private float timer;
    public Rigidbody2D rigid;
    public SpriteRenderer sprite;
    private Enemy parent;
    private void Update()
    {
        timer+=Time.deltaTime;
        if(timer > 4f)
        {
            sprite.color = new(1, 1, 1, sprite.color.a - Time.deltaTime);
        }
        if(timer > 5f)
        {
            Destroy(gameObject);
        }
    }
    private void FixedUpdate()
    {
        if(timer > 1f)
        {
            transform.position = Vector2.MoveTowards(transform.position, GameManager.Inst.player.transform.position, Time.deltaTime * 2f);
        }
        else
        {
            rigid.MovePosition((Vector2)transform.position + Vector2.up * Time.deltaTime * 1.5f);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            GameManager.Inst.player.Hit(parent, 10f);
            Destroy(gameObject);
        }
    }
}
