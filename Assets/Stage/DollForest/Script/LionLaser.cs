using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LionLaser : MonoBehaviour
{
    public bool once = false;
    private float Timer = 0f;
    public Collider2D col;
    public SpriteRenderer sprite;
    public Sprite[] sprites;
    public bool LaserOn = false;
    public TeddyLion parent;
    private void Awake()
    {
        col.enabled = false;
        sprite.color = new(1, 1, 1, 0);
        sprite.sprite = sprites[0];
    }
    private void Start()
    {
        transform.localPosition = Vector2.zero;
    }
    private void Update()
    {
        if(!LaserOn)
        {
            sprite.color = new(1, 1, 1, sprite.color.a + Time.deltaTime / 3);
            if(sprite.color.a >= 1)
            {
                LaserOn = true;
                sprite.sprite = sprites[1];
                col.enabled = true;
                transform.localScale = new(500f, 1f);
            }
        }
        else
        {
            Timer += Time.deltaTime;
            if(Timer > 0.5f)
            {
                sprite.color = new(1, 1, 1, sprite.color.a - Time.deltaTime * 10);
                if(once)
                {
                    col.enabled = false;
                    parent.LaserOn();
                    once = false;
                }
            }
            if(sprite.color.a <= 0)
            {
                Destroy(gameObject);
            }
        }

        if (!parent.sprite.enabled)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !GameManager.Inst.player.Invincible)
        {
            GameManager.Inst.player.Hit(parent, 10);
        }
    }
}
