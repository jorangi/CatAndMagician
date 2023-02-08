using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class RevolutionGem : Bullet
{
    protected override void FixedUpdate()
    {
        transform.RotateAround(transform.parent.position, Vector3.back, Mathf.CeilToInt(Time.fixedDeltaTime) * Speed * GameManager.Inst.player.BulletSpeedRatio * (entity as RevolutionGemSpawner).RevSpeed);
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.CompareTag("Enemy"))
        {
            if(entity.Evo)
            {
                (entity as RevolutionGemSpawner).RevSpeed += ItemManager.ConvertJToken<float>(entity.data.value["val0"])[0] * 0.01f;
            }
        }
    }
}
