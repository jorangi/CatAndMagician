using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlankParchment : Item
{
    private float timer;
    private float timer2;
    private void Update()
    {
        timer -= Time.deltaTime;
        timer2 -= Time.deltaTime;
        if(timer <= 0)
        {
            StartCoroutine(Buff(Random.Range(0, 3), ItemManager.ConvertJToken<float>(data.value["val0"])[5]));
            timer = ItemManager.ConvertJToken<float>(data.value["val0"])[Mathf.Min(ItemManager.ConvertJToken<float>(data.value["val0"]).Length - 1, Lv - 1)];
        }
        if(timer2 <= 0 && Evo)
        {
            StartCoroutine(Buff(0, ItemManager.ConvertJToken<float>(data.value["val1"])[1]));
            StartCoroutine(Buff(1, ItemManager.ConvertJToken<float>(data.value["val1"])[1]));
            StartCoroutine(Buff(2, ItemManager.ConvertJToken<float>(data.value["val1"])[1]));
            timer2 = ItemManager.ConvertJToken<float>(data.value["val1"])[0];
        }
    }
    private IEnumerator Buff(int type, float time)
    {
        if(type == 0)
        {
            GameManager.Inst.player.BulletDmgRatio *= 1 + ItemManager.ConvertJToken<float>(data.value["val0"])[6] * 0.01f;
            yield return new WaitForSeconds(time);
            GameManager.Inst.player.BulletDmgRatio /= 1 + ItemManager.ConvertJToken<float>(data.value["val0"])[6] * 0.01f;
        }
        else if(type == 1)
        {
            GameManager.Inst.player.DelayRatio *= 1 + ItemManager.ConvertJToken<float>(data.value["val0"])[7] * 0.01f;
            yield return new WaitForSeconds(time);
            GameManager.Inst.player.DelayRatio /= 1 + ItemManager.ConvertJToken<float>(data.value["val0"])[7] * 0.01f;
        }
        else
        {
            GameManager.Inst.player.BulletSpeedRatio *= 1 + ItemManager.ConvertJToken<float>(data.value["val0"])[8] * 0.01f;
            yield return new WaitForSeconds(time);
            GameManager.Inst.player.BulletSpeedRatio /= 1 + ItemManager.ConvertJToken<float>(data.value["val0"])[8] * 0.01f;
        }
    }
}
