using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBall : Bullet
{
    private Rigidbody2D rigid;
    public int bounce;
    private GameObject colObj;
    protected override void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        GetComponent<BoxCollider2D>().isTrigger = true;
    }
    private void OnDisable()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
        rigid.velocity = Vector3.zero;
    }
    protected override void Update()
    {
        rigid.velocity = speed * transform.up;
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.CompareTag("Remove"))
        {
            ReturnObject();
        }
        if (collision == null)
        {
            if(collision.CompareTag("Enemy"))
            {
                if (collision.GetComponent<Enemy>() != null)
                {
                    collision.transform.Translate(Vector2.up * knockback);
                    collision.GetComponent<Enemy>().HP -= dmg;
                }
                else if (collision.GetComponent<Boss>() != null)
                {
                    collision.GetComponent<Boss>().HP -= dmg;
                }
                if (bounce > 0)
                {
                    transform.rotation = Quaternion.Euler(0, 0, Random.Range(0f, 361f));
                    bounce--;
                }
                else
                {
                    ReturnObject();
                }
                colObj = collision.gameObject;
            }
            else if(collision.CompareTag("Border") && colObj != collision.gameObject)
            {
                if (bounce > 0)
                {
                    switch (collision.name)
                    {
                        case "above":
                            transform.rotation = Quaternion.Euler(0, 0, Random.Range(151, 209f));
                            break;
                        case "below":
                            transform.rotation = Quaternion.Euler(0, 0, Random.Range(-29f, 29f));
                            break;
                        case "left":
                            transform.rotation = Quaternion.Euler(0, 0, Random.Range(-119f, -59f));
                            break;
                        case "right":
                            transform.rotation = Quaternion.Euler(0, 0, Random.Range(61f, 119f));
                            break;
                    }
                    bounce--;
                }
                colObj = collision.gameObject;
            }
        }
        else
        {
            if (collision.CompareTag("Enemy") && colObj != collision.gameObject)
            {
                if (collision.GetComponent<Enemy>() != null)
                {
                    collision.transform.Translate(Vector2.up * knockback);
                    collision.GetComponent<Enemy>().HP -= dmg;
                }
                else if (collision.GetComponent<Boss>() != null)
                {
                    collision.GetComponent<Boss>().HP -= dmg;
                }
                if (bounce > 0)
                {
                    transform.rotation = Quaternion.Euler(0, 0, Random.Range(0f, 361f));
                    bounce--;
                }
                else
                {
                    ReturnObject();
                }
                colObj = collision.gameObject;
            }
            else if (collision.CompareTag("Border") && colObj != collision.gameObject)
            {
                if(bounce > 0)
                {
                    switch (collision.name)
                {
                        case "above":
                            transform.rotation = Quaternion.Euler(0, 0, Random.Range(151, 209f));
                            break;
                        case "below":
                            transform.rotation = Quaternion.Euler(0, 0, Random.Range(-29f, 29f));
                            break;
                        case "left":
                            transform.rotation = Quaternion.Euler(0, 0, Random.Range(-119f, -59f));
                            break;
                        case "right":
                            transform.rotation = Quaternion.Euler(0, 0, Random.Range(61f, 119f));
                            break;
                    }
                    bounce--;
                }
                colObj = collision.gameObject;
            }
        }
    }
}
