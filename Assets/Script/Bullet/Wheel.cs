using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheel : WideBullet
{
    protected override void FixedUpdate()
    {
        if (transform.position.x <= 5.5f)
        {
            transform.Translate(8f * Time.fixedDeltaTime * Vector2.right);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
