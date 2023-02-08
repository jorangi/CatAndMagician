using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InlaidTopazRingSpawner : BulletSpawner
{
    protected override void Evolved()
    {
        base.Evolved();
        if (!GameManager.Inst.player.itemEvos["FragmentOfMonster"])
        {
            GameManager.Inst.player.Items["FragmentOfMonster"].Evo = true;
        }
    }
}
