using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InlaidGarnetRingSpawner : BulletSpawner
{
    protected override void Evolved()
    {
        base.Evolved();
        if (!GameManager.Inst.player.itemEvos["JewelryPomegranate"])
        {
            GameManager.Inst.player.Items["JewelryPomegranate"].Evo = true;
        }
    }
}
