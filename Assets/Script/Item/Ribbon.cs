using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ribbon : Item
{
    private float timer = 0f;
    private float shieldValue = 0f;
    private void Update()
    {
        if (GameManager.Inst.player.RibbonShield > shieldValue || !Evo)
        {
            return;
        }
        timer += Time.deltaTime;
        if(timer > 10f)
        {
            timer = 0f;
            shieldValue = GameManager.Inst.player.maxhp * 0.1f;
            GameManager.Inst.player.RibbonShield += GameManager.Inst.player.ChangeHS(false, shieldValue);
            GameManager.Inst.player.Shield += GameManager.Inst.player.ChangeHS(false, shieldValue);
        }
    }
    protected override void LevelChanged()
    {
        base.LevelChanged();
        GameManager.Inst.player.MaxHPRatio *= 1 + ItemManager.ConvertJToken<float>(data.value["val0"])[Mathf.Min(ItemManager.ConvertJToken<float>(data.value["val0"]).Length - 1, Lv - 1)] * 0.01f;
    }
}
