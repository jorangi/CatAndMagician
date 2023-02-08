using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InlaidOpalRingSpawner : BulletSpawner
{
    protected override void Evolved()
    {
        base.Evolved();
        if (!GameManager.Inst.player.itemEvos["JewelryCuckoosEgg"])
        {
            GameManager.Inst.player.Items["JewelryCuckoosEgg"].Evo = true;
        }
    }
}
