using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class InlaidAquamarineRingSpawner : BulletSpawner
{
    private float abilityVal = 1.0f;
    private float delayAdd = 0f;
    protected override void Awake()
    {
        base.Awake();
        delayAdd = ItemManager.ConvertJToken<float>(data.value["val0"])[0] * 0.01f;
    }
    protected override void Update()
    {
        if (Lv == 0 && name == "BulletSpawner" || GameManager.Inst.player.stopped)
            return;
        timer += Time.deltaTime;
        if (name == "BulletSpawner")
        {
            abilityVal = 1.0f;
            for (int i = 0; i < GameManager.Inst.player.Tears.Count; i++)
            {
                int j = GameManager.Inst.player.Tears.Keys.ToArray()[i];
                if (GameManager.Inst.player.Tears[j] > 0)
                {
                    abilityVal = 1 + delayAdd;
                    break;
                }

            }
            if (timer >= (1 / (delay * GameManager.Inst.player.DelayRatio * abilityVal + GameManager.Inst.player.NumericalDelay)))
            {
                timer = 0;
                ShootBullet();
            }
        }
        else
        {
            if (timer >= (1 / delay))
            {
                timer = 0;
                ShootBullet();
            }
        }
    }
    protected override void Evolved()
    {
        base.Evolved();
        if (!GameManager.Inst.player.itemEvos["TearOfTheSea"])
        {
            GameManager.Inst.player.Items["TearOfTheSea"].Evo = true;
        }
    }
}
