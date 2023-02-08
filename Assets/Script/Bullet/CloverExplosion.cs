using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloverExplosion : WideBullet
{
    private float alpha = 1f;
    private void Update()
    {
        alpha -= Time.deltaTime * 4;
        sprite.color = new(1, 1, 1, alpha);
        if (alpha <= 0)
        {
            ReturnObject();
        }
        else if (alpha <= 0.9f)
        {
            col.enabled = false;
        }
    }
    protected override void FixedUpdate()
    {
    }
}
