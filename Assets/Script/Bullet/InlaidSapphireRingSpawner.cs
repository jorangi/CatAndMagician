using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InlaidSapphireRingSpawner : BulletSpawner
{
    public GameObject SapphireWall;
    protected override void Evolved()
    {
        base.Evolved();
        if (!GameManager.Inst.player.itemEvos["SapphireEncrustedStatue"])
        {
            GameManager.Inst.player.Items["SapphireEncrustedStatue"].Evo = true;
        }
    }
}
