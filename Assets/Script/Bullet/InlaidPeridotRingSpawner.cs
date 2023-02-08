using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InlaidPeridotRingSpawner : BulletSpawner
{
    protected override void Evolved()
    {
        base.Evolved();
        if (!GameManager.Inst.player.itemEvos["JewelryFlower"])
        {
            GameManager.Inst.player.Items["JewelryFlower"].Evo = true;
        }
    }
}
