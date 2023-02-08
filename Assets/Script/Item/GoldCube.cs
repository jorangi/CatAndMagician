using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldCube : Item
{
    private float AddDmg = 0f;
    private int gold;
    public int Gold
    {
        get => gold;
        set
        {
            while(value >= 100)
            {
                GameManager.Inst.player.GoldCubeItems = true;
                GameManager.Inst.player.levelupCount++;
                value -= 100;
            }
            gold = value;
        }
    }
    private void Update()
    {
        GameManager.Inst.player.NumericalBulletDmg -= AddDmg;
        AddDmg = (GameManager.Inst.player.maxhp - GameManager.Inst.player.playerHP) / 10f;
        GameManager.Inst.player.NumericalBulletDmg += AddDmg;
    }
}
