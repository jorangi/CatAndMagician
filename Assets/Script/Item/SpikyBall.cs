using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikyBall : Item
{
    private int lv;
    public int Lv
    {
        get => lv;
        set
        {
            enabled = true;
            value = Mathf.Clamp(value, 1, data.val.Length);
            if (GameManager.Inst.player.SpikeDmg == 0)
            {
                GameManager.Inst.player.SpikeDmg = 10;
            }
            else
            {
                GameManager.Inst.player.SpikeDmg *= data.val[value - 1];
            }
            lv = value;
            GameManager.Inst.player.itemLevels["SpikyBall"] = value;
        }
    }
    public override void SetLv(int lv)
    {
        Lv = lv;
    }
    public override void AddLv()
    {
        Lv++;
    }
    public override void SubLv()
    {
        Lv--;
    }
}
