using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InlaidGoldRingSpawner : BulletSpawner
{
    protected override void Evolved()
    {
        base.Evolved();
        if (!GameManager.Inst.player.itemEvos["GoldCube"])
        {
            GameManager.Inst.player.Items["GoldCube"].Evo = true;
        }
    }
}
