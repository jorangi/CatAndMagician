using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PumpkinPie : Item
{
    private bool fullHP = false;
    public bool FullHP
    {
        get => fullHP;
        set
        {
            if (value != fullHP)
            {
                if(value)
                {
                    GameManager.Inst.player.DelayRatio *= 1 + ItemManager.ConvertJToken<float>(data.value["val1"])[0] * 0.01f;
                }
                else
                {
                    GameManager.Inst.player.DelayRatio /= 1 + ItemManager.ConvertJToken<float>(data.value["val1"])[0] * 0.01f;
                }
            }
            fullHP = value;
        }
    }
    protected override void LevelChanged()
    {
        base.LevelChanged();
        GameManager.Inst.player.MaxHPRatio *= 1 + ItemManager.ConvertJToken<float>(data.value["val0"])[0] * 0.01f;
        GameManager.Inst.player.Recover *= 1 + ItemManager.ConvertJToken<float>(data.value["val0"])[1] * 0.01f;
    }
}
