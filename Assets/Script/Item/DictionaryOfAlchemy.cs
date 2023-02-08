using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DictionaryOfAlchemy : Item
{
    private float timer;
    private void Update()
    {
        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            timer = ItemManager.ConvertJToken<float>(data.value["val1"])[4];
            Debug.Log("Æ÷¼Ç ÅõÃ´");
        }
    }
    protected override void LevelChanged()
    {
        base.LevelChanged();
        GameManager.Inst.player.MaxHPRatio *= 1 + ItemManager.ConvertJToken<float>(data.value["val0"])[0] * 0.01f;
        GameManager.Inst.player.BulletSpeedRatio *= 1 + ItemManager.ConvertJToken<float>(data.value["val0"])[1] * 0.01f;
    }
}
