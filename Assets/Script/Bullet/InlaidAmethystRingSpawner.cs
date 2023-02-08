using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InlaidAmethystRingSpawner : BulletSpawner
{
    protected override void Evolved()
    {
        base.Evolved();
        if (!GameManager.Inst.player.itemEvos["GrapeBrooch"])
        {
            GameManager.Inst.player.Items["GrapeBrooch"].Evo = true;
        }
    }
}
