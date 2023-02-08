using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirstrikeRange : Bullet
{
    public SpriteRenderer red;
    public float timer;

    protected override void OnEnable()
    {
        base.OnEnable();
        red.sortingOrder = sprite.sortingOrder - 1;
        StartCoroutine(ExpandingRange());
    }
    private IEnumerator ExpandingRange()
    {
        timer = Mathf.Max(1 / speed, 0f);
        red.transform.localScale = new(0, 0, 1);
        float size = 0;
        while(timer > 0)
        {
            size += Time.deltaTime * speed;
            red.transform.localScale = new(size, size, 1);
            timer -= Time.deltaTime;
            yield return null;
        }
        DeployMagicCircle();
    }
    private void DeployMagicCircle()
    {
        GameObject obj = GameObject.FindGameObjectWithTag("Bullets").transform.Find(entity.data.value["id"].ToString()).GetChild(0).gameObject;
        obj.transform.SetParent(null);
        Bullet bullet = obj.GetComponent<Bullet>();
        bullet.Bullets = GameObject.FindGameObjectWithTag("Bullets").transform.Find(entity.data.value["id"].ToString());
        bullet.entity = spawner;
        obj.transform.localScale = transform.localScale;
        obj.name = spawner.data.value["id"].ToString();
        obj.transform.position = transform.position;
        obj.SetActive(true);
        StartCoroutine(Hide());
    }
    private IEnumerator Hide()
    {
        float t = 0;
        float alpha = 0;
        var b = transform.GetChild(0).GetComponent <SpriteRenderer>();
        while(t < 0.3f)
        {
            alpha += Time.deltaTime * (1/0.3f);
            sprite.color = new(1, 1, 1, 1 - alpha);
            b.color = new(1, 1, 1, 1 - alpha);
            t += Time.deltaTime;
            yield return null;
        }
        transform.SetParent(Bullets);
        gameObject.SetActive(false);
        sprite.color = new(1, 1, 1, 1);
        b.color = new(1, 1, 1, 1);
    }
    protected override void FixedUpdate()
    {
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
    }
    protected override void OnTriggerExit2D(Collider2D collision)
    {
    }
}
