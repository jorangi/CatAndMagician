using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeddyLion : Enemy
{
    private readonly WaitForSeconds stay = new(0.5f);
    public Sprite Alert;
    public Sprite Laser;
    protected override void Awake()
    {
        base.Awake();
        StartCoroutine(XLaser());
    }
    private void Start()
    {
        transform.position = new(0, 5.5f);
    }
    private IEnumerator XLaser()
    {
        GameObject l1 = new();
        GameObject l2 = new();
        l1.layer = 11;
        l2.layer = 11;
        SpriteRenderer l1s = l1.AddComponent<SpriteRenderer>();
        SpriteRenderer l2s = l2.AddComponent<SpriteRenderer>();
        l1.AddComponent<Rigidbody2D>().gravityScale = 0;
        l2.AddComponent<Rigidbody2D>().gravityScale = 0;
        l1.AddComponent<LionLaser>();
        l2.AddComponent<LionLaser>();
        l1s.sprite = Alert;
        l2s.sprite = Alert;
        l1.transform.localScale = new(1000, 1);
        l2.transform.localScale = new(1000, 1);
        l1.transform.rotation = Quaternion.Euler(0, 0, -45);
        l2.transform.rotation = Quaternion.Euler(0, 0, 45);
        l1.transform.SetParent(transform);
        l2.transform.SetParent(transform);
        l1.transform.localPosition = new(0, 0);
        l2.transform.localPosition = new(0, 0);
        float a = 0;
        while (a < 1)
        {
            a += Time.deltaTime / 3;
            l1s.color = new(1, 1, 1, a);
            l2s.color = new(1, 1, 1, a);
            yield return null;
        }
        a = 1;

        l1s.sprite = Laser;
        l2s.sprite = Laser;
        l1.AddComponent<BoxCollider2D>().isTrigger = true;
        l2.AddComponent<BoxCollider2D>().isTrigger = true;
        yield return stay;
        StartCoroutine(PlusLaser());
        while (l1s.color.a > 0)
        {
            a -= Time.deltaTime * 10;
            l1s.color = new(1, 1, 1, a);
            l2s.color = new(1, 1, 1, a);
            yield return null;
        }
        Destroy(l1);
        Destroy(l2);
    }
    private IEnumerator PlusLaser()
    {
        GameObject l1 = new();
        GameObject l2 = new();
        l1.layer = 11;
        l2.layer = 11;
        SpriteRenderer l1s = l1.AddComponent<SpriteRenderer>();
        SpriteRenderer l2s = l2.AddComponent<SpriteRenderer>();
        l1.AddComponent<Rigidbody2D>().gravityScale = 0;
        l2.AddComponent<Rigidbody2D>().gravityScale = 0;
        l1.AddComponent<LionLaser>();
        l2.AddComponent<LionLaser>();
        l1s.sprite = Alert;
        l2s.sprite = Alert;
        l1.transform.localScale = new(1000, 1);
        l2.transform.localScale = new(1000, 1);
        l2.transform.rotation = Quaternion.Euler(0, 0, 90);
        l1.transform.SetParent(transform);
        l2.transform.SetParent(transform);
        l1.transform.localPosition = new(0, 0);
        l2.transform.localPosition = new(0, 0);
        float a = 0;
        while (a < 1)
        {
            a += Time.deltaTime / 3;
            l1s.color = new(1, 1, 1, a);
            l2s.color = new(1, 1, 1, a);
            yield return null;
        }
        a = 1;

        l1s.sprite = Laser;
        l2s.sprite = Laser;
        l1.AddComponent<BoxCollider2D>().isTrigger = true;
        l2.AddComponent<BoxCollider2D>().isTrigger = true;
        yield return stay;
        StartCoroutine(XLaser());
        while (l1s.color.a > 0)
        {
            a -= Time.deltaTime * 10;
            l1s.color = new(1, 1, 1, a);
            l2s.color = new(1, 1, 1, a);
            yield return null;
        }
        Destroy(l1);
        Destroy(l2);
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (transform.position.y < -5.5f && collision.CompareTag("Remove"))
        {
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Player") && !GameManager.Inst.player.Invincible)
        {
            if (!SpikeTimer)
            {
                SpikeTimer = true;
                HP -= GameManager.Inst.player.SpikeDmg;
            }
            GameManager.Inst.player.HP -= dmg;
        }
    }
}
