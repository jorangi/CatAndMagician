using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JewelryPomegranate : Item
{
    public int Resurrect = 0;
    protected override void LevelChanged()
    {
        base.LevelChanged();
        GameManager.Inst.player.MaxHPRatio *= 1 + ItemManager.ConvertJToken<float>(data.value["val0"])[0] * 0.01f;
        GameManager.Inst.player.BulletDmgRatio *= 1 + ItemManager.ConvertJToken<float>(data.value["val0"])[1] * 0.01f;
    }
    protected override void Evolved()
    {
        base.Evolved();
        Resurrect += ItemManager.ConvertJToken<int>(data.value["val1"])[0];
    }
}
