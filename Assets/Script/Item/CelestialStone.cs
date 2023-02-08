using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CelestialStone : Item
{
    private float EvoDelay = 1.0f;
    protected override void LevelChanged()
    {
        base.LevelChanged();
        GameManager.Inst.player.DelayRatio *= 1 + ItemManager.ConvertJToken<float>(data.value["val0"])[Mathf.Min(ItemManager.ConvertJToken<float>(data.value["val0"]).Length - 1, Lv - 1)] * 0.01f;
    }
    protected override void Evolved()
    {
        base.Evolved();
        CelestialEvo();
    }
    public void CelestialEvo()
    {
        GameManager.Inst.player.DelayRatio /= EvoDelay;
        EvoDelay = 2 - GameManager.Inst.player.HP / GameManager.Inst.player.maxhp;
        GameManager.Inst.player.DelayRatio *= EvoDelay;
    }
}
