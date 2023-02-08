using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigRuby : Item
{
    protected override void LevelChanged()
    {
        base.LevelChanged();
        GameManager.Inst.player.MaxHPRatio *= 1 + ItemManager.ConvertJToken<float>(data.value["val0"])[0] * 0.01f;
    }
}
