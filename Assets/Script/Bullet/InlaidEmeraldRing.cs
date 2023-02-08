using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InlaidEmeraldRing : InlaidProjectile
{
    public float timer = 0f;
    private float lifeTime = 0f;
    public EmeraldLaser laser;
    private bool adhension = false;
    protected override void FixedUpdate()
    {
        if(!adhension)
        {
            base.FixedUpdate();
        }
    }
    private void Update()
    {
        timer -= Time.deltaTime;

        if(timer <= 0f)
        {
            timer = ItemManager.ConvertJToken<float>(entity.data.value["val0"])[0];
            laser.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
            laser.gameObject.SetActive(true);
            laser.dmg = dmg;
            laser.timer = 1f;
        }
        if(lifeTime > 0f)
        {
            lifeTime -= Time.deltaTime;
        }
        if (lifeTime < 0f)
        {
            laser.gameObject.SetActive(false);
            (entity as InlaidEmeraldRingSpawner).emerald.Remove(this);
            ReturnObject();
        }
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        InlaidEmeraldRingSpawner spawner = entity as InlaidEmeraldRingSpawner;
        if(collision.CompareTag("Border") && entity.Evo && spawner.emerald.Count < spawner.emerald.Capacity)
        {
            spawner.emerald.Add(this);
            adhension = true;
            lifeTime = ItemManager.ConvertJToken<float>(entity.data.value["val1"])[1];
        }
    }
}
