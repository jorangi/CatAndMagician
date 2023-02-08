using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InlaidEmeraldRingSpawner : BulletSpawner
{
    public List<InlaidEmeraldRing> emerald = new();
    protected override void Awake()
    {
        base.Awake();
        emerald.Capacity = ItemManager.ConvertJToken<int>(data.value["val1"])[0];
    }
    protected override void ShootBullet(Bullet bullet, Transform parent, string name)
    {
        base.ShootBullet(bullet, parent, name);
        (bullet as InlaidEmeraldRing).timer = ItemManager.ConvertJToken<float>(data.value["val0"])[0];
    }
    protected override void Evolved()
    {
        base.Evolved();
        if (!GameManager.Inst.player.itemEvos["EmeraldRabbitsTail"])
        {
            GameManager.Inst.player.Items["EmeraldRabbitsTail"].Evo = true;
        }
    }
}
