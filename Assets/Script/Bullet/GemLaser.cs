using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemLaser : WideBullet
{
    public RevolutionGemSpawner jamSpawner;
    private float dmgRatio = 0.0f;
    private float Delay = 0.0f;
    private float delay = 0.0f;
    private void Start()
    {
        jamSpawner = entity as RevolutionGemSpawner;
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        transform.localScale = new(1, transform.localScale.y);
        Delay = 0.0f;
    }
    private void Update()
    {
        if (jamSpawner.Reflecting)
        {
            float[] t = ItemManager.ConvertJToken<float>(jamSpawner.data.value["val0"]);
            delay = t[5];
            dmgRatio = 1 + t[6] * 0.01f;

            Delay += Time.deltaTime;
            if(Delay >= delay)
            {
                sprite.color = new(1, 1, 1, 1f);
                transform.localScale = new(1, transform.localScale.y);
                col.enabled = true;
                dmg = (entity as BulletSpawner).dmg * dmgRatio;
                Delay = 0.0f;
                PierceCheck(true);
            }
            else
            {
                sprite.color = new(1, 1, 1, Mathf.Lerp(sprite.color.a, 0.7f, Delay));
                transform.localScale = new(Mathf.Lerp(transform.localScale.x, 0.1f, Delay), transform.localScale.y);
                if(Delay >= delay * 0.2f)
                {
                    col.enabled = false;
                }
            }
        }
        else
        {
            sprite.color = new(1, 1, 1, Mathf.Pow(sprite.color.a, 2));
            if(sprite.color.a == 0)
            {
                ReturnObject();
            }
        }
    }
    protected override void FixedUpdate()
    {
    }
}
