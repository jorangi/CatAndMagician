using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InlaidDiamondRing : Bullet
{
    public override Item entity
    {
        get => _entity;
        set
        {
            _entity = value;
            spawner = value as BulletSpawner;
            if (spawner != null)
            {
                dmg = spawner.dmg;
                speed = spawner.speed;
            }
            transform.GetChild(0).GetComponent<DiamondBullet>().entity = entity;
            transform.GetChild(1).GetComponent<DiamondBullet>().entity = entity;
        }
    }
    protected override void FixedUpdate()
    {
        transform.Translate(Speed * Time.fixedDeltaTime * Vector2.up, Space.World);
        if (transform.position.y > 7f)
        {
            ReturnObject();
        }
        else if (transform.position.y < -7f)
        {
            ReturnObject();
        }
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Remove"))
        {
            ReturnObject();
        }
        if (collision.gameObject == inPortal && rebuild)
        {
            transform.position = outPortal.transform.position;
            Destroy(inPortal);
            Destroy(outPortal);
        }
    }
}
