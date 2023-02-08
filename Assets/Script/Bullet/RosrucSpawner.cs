using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RosrucSpawner : BulletSpawner
{
    public Wheel Wheel;
    public Transform HittedEnemy;
    private int hitCount;
    public int HitCount
    {
        get => hitCount;
        set
        {
            hitCount = value;
            if(hitCount == (int)ItemManager.ConvertJToken<float>(data.value["val0"])[0] && Evo)
            {
                GameObject obj = Instantiate(Wheel.gameObject);
                Wheel.dmg = ItemManager.ConvertJToken<float>(data.value["val0"])[1] * GameManager.Inst.player.BulletDmgRatio;
                if(HittedEnemy != null)
                    obj.transform.position = new (-5f, HittedEnemy.transform.position.y);
                else
                    obj.transform.position = new(-5f, transform.position.y);
                hitCount = 0;
            }
        }
    }
}
