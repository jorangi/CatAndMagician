using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBroom : Item
{
    private int broomCount;
    private float timer;
    private float timer2;
    private void Update()
    {
        timer -= Time.deltaTime;
        timer2 -= Time.deltaTime;
        if(timer <= 0)
        {
            for(int i = 0; i < broomCount; i++)
            {
                FindObjectOfType<EnemyProjectile>()?.Remove();
                timer = ItemManager.ConvertJToken<float>(data.value["val0"])[^1];
            }
            timer = 0;
        }
        if(timer2 <= 0)
        {
            foreach(Enemy e in FindObjectsOfType<Enemy>())
            {
                e.GetCC("MagicBroom", "grabbed", new Vector2(e.transform.position.x, 5f), 5f);
            }
            timer2 = ItemManager.ConvertJToken<float>(data.value["val1"])[0];
        }
    }
    protected override void LevelChanged()
    {
        base.LevelChanged();
        broomCount = ItemManager.ConvertJToken<int>(data.value["val0"])[Mathf.Min(ItemManager.ConvertJToken<int>(data.value["val0"]).Length - 1, Lv - 1)];
        timer = ItemManager.ConvertJToken<float>(data.value["val0"])[^1];
    }
}
