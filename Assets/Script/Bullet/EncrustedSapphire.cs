using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncrustedSapphire : Bullet
{
    private Enemy TriggerEnemy;
    private float t = 0.5f;
    private float spd = 7f;
    protected override void Awake()
    {
        base.Awake();
        col.enabled = false;
        transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 361));
        entity = GameManager.Inst.player.Items["SapphireEncrustedStatue"];
        dmg = 0;
        spd = 7f;
    }
    private void Update()
    {
        if (t > 0)
        {
            t -= Time.deltaTime;
        }
        else
        {
            TriggerEnemy = null;
        }
    }
    protected override void Knockback(Enemy enemy)
    {
        if (entity == null)
            return;
        float knockbackvalue = ItemManager.ConvertJToken<float>(entity.data.value["val1"])[0] + GameManager.Inst.player.Knockback;
        enemy.Knockbacked(knockbackvalue);
    }
    protected override void FixedUpdate()
    {
        rigid.MovePosition(transform.position + GameManager.Inst.player.BulletSpeedRatio * spd * Time.fixedDeltaTime * transform.up);
    }
    protected override void HitEnemy()
    {
        for (int i = 0; i < HitTarget.Count; i++)
        {
            if (HitTarget[i] == TriggerEnemy)
            {
                continue;
            }
            Knockback(HitTarget[i]);
            HitEnemy(HitTarget[i]);
            if (i >= MaxPierce)
            {
                break;
            }
        }
        HitTarget.Clear();
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.CompareTag("Enemy"))
        {
            TriggerEnemy = collision.GetComponent<Enemy>();
        }
    }
}
