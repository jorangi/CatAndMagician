using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBallSpawner : BulletSpawner
{
    public GameObject Frisbee;

    private float Timer = 0f;
    public int bounce = 0;

    protected override void Update()
    {
        base.Update();
        if(Timer > 0 && Evo)
        {
            Timer -= Time.deltaTime;
        }
        else if(Timer <= 0 && Evo)
        {
            Timer = 30.0f; 
            GameObject obj = Instantiate(Frisbee);
            obj.GetComponent<Frisbee>().dmg = ItemManager.ConvertJToken<float>(data.value["val1"])[2] * GameManager.Inst.player.BulletDmgRatio;
        }
    }
    protected override void LevelChanged()
    {
        base.LevelChanged();
        bounce = ItemManager.ConvertJToken<int>(data.value["val0"])[Mathf.Min(Lv - 1, ItemManager.ConvertJToken<int>(data.value["val0"]).Length - 1)];
    }
    protected override void ShootBullet(Bullet bullet, Transform parent, string name)
    {
        base.ShootBullet(bullet, parent, name);
        (bullet as MagicBall).bounce = bounce;
    }
}
