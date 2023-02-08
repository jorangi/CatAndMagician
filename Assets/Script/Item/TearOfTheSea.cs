using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TearOfTheSea : Item
{
    public GameObject Tear;
    private float timer = 0f;

    private void Update()
    {
        timer -= Time.deltaTime;
        if(timer <= 0f)
        {
            timer = ItemManager.ConvertJToken<float>(data.value["val0"])[Mathf.Min(ItemManager.ConvertJToken<float>(data.value["val0"]).Length - 1, Lv - 1)];
            GameObject tear = Instantiate(Tear);
            tear.transform.position = new(Random.Range(-2.5f, 2.5f), Random.Range(-3f, 4.25f));
        }
    }
    protected override void LevelChanged()
    {
        base.LevelChanged();
        GameManager.Inst.player.BulletSpeedRatio /= 1 + ItemManager.ConvertJToken<float>(data.value["val0"])[Mathf.Min(ItemManager.ConvertJToken<float>(data.value["val0"]).Length - 1, Lv - 1)] * 0.01f;
    }
}
