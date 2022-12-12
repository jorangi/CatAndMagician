using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Churu : Item
{
    private int lv;
    public int Lv
    {
        get => lv;
        set
        {
            enabled = true;
            value = Mathf.Clamp(value, 1, data.val.Length);
            if (GameManager.Inst.player.recover == 0)
            {
                GameManager.Inst.player.recover = 1;
            }
            else
            {
                GameManager.Inst.player.recover *= data.val[value - 1];
            }
            lv = value;
            GameManager.Inst.player.itemLevels["Churu"] = value;
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
