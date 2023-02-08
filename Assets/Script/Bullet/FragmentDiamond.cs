using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FragmentDiamond : Bullet
{
    private Enemy TriggerEnemy;
    private float t = 0.5f;
    protected override void Awake()
    {
        base.Awake();
        col.enabled = false;
        entity = GameManager.Inst.player.Items["BrokenWatch"];
        dmg = ItemManager.ConvertJToken<float>(entity.data.value["val0"])[0];
        transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 361));
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
    protected override void FixedUpdate()
    {
        rigid.MovePosition(transform.position + GameManager.Inst.player.BulletSpeedRatio * 7 * Time.fixedDeltaTime * transform.up);
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
