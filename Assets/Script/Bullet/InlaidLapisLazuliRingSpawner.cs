using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InlaidLapisLazuliRingSpawner : BulletSpawner
{
    private WaitForSeconds d;
    private float AbilityVal = 1.0f;
    public Coroutine OnAbility;
    protected override void Awake()
    {
        base.Awake();
        d = new(ItemManager.ConvertJToken<float>(data.value["val0"])[0]);
    }
    protected override void Update()
    {
        if (Lv == 0 && name == "BulletSpawner" || GameManager.Inst.player.stopped)
            return;


        timer += Time.deltaTime;
        if (name == "BulletSpawner")
        {
            if(Evo)
            {
                CurrentDelay = (1 / (delay * GameManager.Inst.player.DelayRatio * AbilityVal * (1 + ItemManager.ConvertJToken<float>(data.value["val1"])[0] * GameManager.Inst.player.InlaidCount * 0.01f) + GameManager.Inst.player.NumericalDelay));
                if (timer >= CurrentDelay)
                {
                    timer = 0;
                    ShootBullet();
                }
            }
            else
            {
                CurrentDelay = (1 / (delay * GameManager.Inst.player.DelayRatio * AbilityVal + GameManager.Inst.player.NumericalDelay));
                if (timer >= CurrentDelay)
                {
                    timer = 0;
                    ShootBullet();
                }
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
    public IEnumerator Ability()
    {
        AbilityVal = 1 + ItemManager.ConvertJToken<float>(data.value["val0"])[1] * 0.01f;
        yield return d;
        AbilityVal = 1f;
    }
    protected override void Evolved()
    {
        base.Evolved();
        if (!GameManager.Inst.player.itemEvos["CelestialStone"])
        {
            GameManager.Inst.player.Items["CelestialStone"].Evo = true;
        }
    }
}
