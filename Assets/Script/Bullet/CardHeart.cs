using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardHeart : Bullet
{
    protected override void HitEnemy(float dmg, Enemy enemy)
    {
        base.HitEnemy(dmg, enemy);
        if(enemy.HP <= 0)
        {
            GameManager.Inst.player.HP += GameManager.Inst.player.ChangeHS(true, GameManager.Inst.player.maxhp * ItemManager.ConvertJToken<float>(ItemManager.datas["BrokenWatch"].value["val0"])[2] * 0.01f);
        }
    }
}