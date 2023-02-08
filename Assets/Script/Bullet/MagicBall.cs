using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBall : Bullet
{
    public int bounce;
    private GameObject colObj;
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (colObj == null)
        {
            if(collision.CompareTag("Enemy"))
            {
                if (bounce > 0)
                {
                    transform.rotation = Quaternion.Euler(0, 0, Random.Range(0f, 361f));
                    bounce--;
                }
                colObj = collision.gameObject;
            }
            else if(collision.CompareTag("Border"))
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
            }
        }//첫 충돌일 경우
        else
        {
            if (collision.CompareTag("Enemy") && colObj != collision.gameObject)
            {
                colObj = collision.gameObject;
            }
            else if (collision.CompareTag("Border"))
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
            }
        }//첫 충돌이 아닐경우
    }
    protected override IEnumerator ReturnObj()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
        return base.ReturnObj();
    }
}
