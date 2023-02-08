using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapeBrooch : Item
{
    public float EvoResist = 0.0f;
    protected override void LevelChanged()
    {
        base.LevelChanged();
        GameManager.Inst.player.Resist += ItemManager.ConvertJToken<float>(data.value["val0"])[Mathf.Min(ItemManager.ConvertJToken<float>(data.value["val0"]).Length - 1, Lv - 1)] * 0.01f;
    }
    public void GrapeBroochEvo()
    {
        GameManager.Inst.player.Resist -= EvoResist;
        EvoResist = GameManager.Inst.player.HP / GameManager.Inst.player.maxhp * 0.5f;
        GameManager.Inst.player.Resist += EvoResist;
    }
}
