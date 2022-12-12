using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Airstrike : Bullet
{
    private void OnEnable()
    {
        StartCoroutine(Hide());
    }
    private IEnumerator Hide()
    {
        yield return new WaitForSeconds(0.05f);
        GetComponent<BoxCollider2D>().enabled = false;
        float t = 0;
        var r = GetComponent<SpriteRenderer>();
        while (t < 0.95f)
        {
            t += Time.deltaTime;
            r.color = new(1, 1, 1, 1-t);
            yield return null;
        }
        ReturnObject();
        r.color = new(1, 1, 1, 1);
        GetComponent<BoxCollider2D>().enabled = true;
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
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
        }
    }
}
