using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HamWheel : WideBullet
{
    protected override void FixedUpdate()
    {
        if (transform.position.y <= 7f)
        {
            transform.Translate(GameManager.Inst.player.MoveSpeed * GameManager.Inst.player.MoveSpeedRatio * Time.fixedDeltaTime * Vector2.up);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
