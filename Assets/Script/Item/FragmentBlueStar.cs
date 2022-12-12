using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FragmentBlueStar : Item
{
    public CircleCollider2D magnet;
    private int lv;
    public int Lv
    {
        get => lv;
        set
        {
            enabled = true;
            magnet.gameObject.SetActive(true);
            value = Mathf.Clamp(value, 1, data.val.Length);
            magnet.radius *= data.val[value-1];
            lv = value;
            GameManager.Inst.player.itemLevels["FragmentBlueStar"] = value;
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
