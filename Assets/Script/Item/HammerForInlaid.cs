using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerForInlaid : Item
{
    public int inlaidMax = 0;
    private int inlaidCount = 0;
    public int InlaidCount
    {
        get => inlaidCount;
        set
        {
            inlaidCount = value;
            EvoAbility = 1 + (value * ItemManager.ConvertJToken<float>(data.value["val1"])[0] * 0.01f);
        }
    }
    private float evoAbility = 1.0f;
    private float EvoAbility 
    {
        get => evoAbility;
        set
        {
            GameManager.Inst.player.BulletDmgRatio /= value;
            evoAbility = value;
            GameManager.Inst.player.BulletDmgRatio *= value;
        }
    }
    protected override void LevelChanged()
    {
        base.LevelChanged();
        inlaidMax = ItemManager.ConvertJToken<int>(data.value["val0"])[Mathf.Min(ItemManager.ConvertJToken<int>(data.value["val0"]).Length - 1, Lv - 1)];
    }
    protected override void Evolved()
    {
        base.Evolved();
        EvoAbility = 1 + (InlaidCount * ItemManager.ConvertJToken<float>(data.value["val1"])[0] * 0.01f);
    }
}
