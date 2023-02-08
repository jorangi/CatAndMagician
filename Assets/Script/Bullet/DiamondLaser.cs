using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiamondLaser : WideBullet
{
    private Animator anim;
    private float Delay = 0.0f;
    private float delay = 0.0f;
    public float timer = 0.0f;
    private bool fire = false;
    protected override void Awake()
    {
        base.Awake();
        sprite.sortingOrder = 39;
        entity = transform.parent.Find("BulletSpawner").GetComponent<InlaidDiamondRingSpawner>();
        anim = GetComponent<Animator>();
    }
    private void Update()
    {
        if (fire)
        {
            col.enabled = false;
            delay = 1 / (GameManager.Inst.player.DelayRatio + GameManager.Inst.player.NumericalDelay);
            timer -= Time.deltaTime;
            Delay += Time.deltaTime;
            if (Delay >= delay)
            {
                float d = 0.0f;
                foreach (KeyValuePair<string, Item> inlaidItem in GameManager.Inst.player.Items)
                {
                    if (inlaidItem.Value.data.isBulletSpawner)
                    {
                        if (inlaidItem.Value.data.value["id"].ToString().IndexOf("Inlaid") > -1 && (inlaidItem.Value as BulletSpawner).enabled)
                        {
                            d += (inlaidItem.Value as BulletSpawner).dmg;
                        }
                    }
                }
                dmg = d;
                Delay = 0.0f;
                col.enabled = true;
                PierceCheck(true);
            }
            if (timer <= 0.0f)
            {
                anim.SetBool("Remove", true);
            }
        }
    }
    public void Fire()
    {
        fire = true;
    }
    public void Hide()
    {
        gameObject.SetActive(false);
        anim.SetBool("Remove", false);
        fire = false;
    }
    protected override void FixedUpdate()
    {
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            HitTarget.Add(collision.transform.parent.GetComponent<Enemy>());
            HitEnemy();
            PierceCheck(PierceAble);
        }
        if (collision.gameObject == inPortal && rebuild)
        {
            transform.position = outPortal.transform.position;
            Destroy(inPortal);
            Destroy(outPortal);
        }
    }
    protected override IEnumerator ReturnObj()
    {
        if (sprite != null)
            sprite.enabled = false;
        col.enabled = false;
        yield return returnDelay;
        gameObject.SetActive(false);
        if (sprite != null)
            sprite.enabled = true;
        col.enabled = true;
    }
}
