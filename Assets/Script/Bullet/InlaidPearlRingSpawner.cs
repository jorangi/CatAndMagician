using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InlaidPearlRingSpawner : BulletSpawner
{
    private float timer2 = 0.0f;
    private float timer3 = 0.0f;
    private BulletSpawner[] spawners;

    protected override void Awake()
    {
        base.Awake();
        spawners = GetComponents<BulletSpawner>();
    }

    protected override void Update()
    {
        if (Lv == 0 && name == "BulletSpawner" || GameManager.Inst.player.stopped)
            return;

        CurrentDelay = 1 / (delay * GameManager.Inst.player.DelayRatio + GameManager.Inst.player.NumericalDelay);

        timer += Time.deltaTime;
        timer2 -= Time.deltaTime;
        timer3 -= Time.deltaTime;

        if(timer2 <= 0f)
        {
            timer2 = ItemManager.ConvertJToken<float>(data.value["val0"])[0];
            timer3 = ItemManager.ConvertJToken<float>(data.value["val0"])[1];
        }
        if(timer3 <= 0)
        {
            foreach (BulletSpawner sp in spawners)
            {
                if(sp.enabled)
                {
                    CurrentDelay = Mathf.Min(sp.CurrentDelay, CurrentDelay);
                }
            }
        }
        if (name == "BulletSpawner")
        {
            if (timer >= CurrentDelay)
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
        if (!GameManager.Inst.player.itemEvos["ABoxOfMemories"])
        {
            GameManager.Inst.player.Items["ABoxOfMemories"].Evo = true;
        }
    }
}
