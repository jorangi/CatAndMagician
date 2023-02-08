using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frisbee : WideBullet
{
    private float timer;
    Vector2 playerPos;

    protected override void Awake()
    {
        timer = ItemManager.ConvertJToken<float>(ItemManager.datas["MagicBall"].value["val1"])[1];
        playerPos = GameManager.Inst.player.transform.position;
        transform.position = new(playerPos.x, -5.5f);
    }
    protected override void FixedUpdate()
    {
        transform.Rotate(0, 0, Time.fixedDeltaTime * 2000f);
        if (timer > 0)
        {
            timer -= Time.fixedDeltaTime;
            transform.position = Vector2.Lerp(transform.position, new(transform.position.x, playerPos.y), Time.fixedDeltaTime * 5f);
        }
        else
        {
            if (transform.position.y > -5.5f)
            {
                transform.position = Vector2.Lerp(transform.position, new(transform.position.x, -6f), Time.fixedDeltaTime * 10f);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy"))
        {
            HitTarget.Add(collision.transform.parent.GetComponent<Enemy>());
            HitEnemy();

            PierceCheck(true);
        }
    }
}
