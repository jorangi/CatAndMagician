using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnifyingGlass : Item
{
    private int lv;
    public int Lv
    {
        get => lv;
        set
        {
            enabled = true;
            value = Mathf.Clamp(value, 1, data.val.Length);
            GameManager.Inst.player.BulletSize *= data.val[value-1];
            lv = value;
            GameManager.Inst.player.itemLevels["MagnifyingGlass"] = value;
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
