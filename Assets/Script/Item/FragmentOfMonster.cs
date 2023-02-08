using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FragmentOfMonster : Item
{
    public GameObject scratch;
    private float timer = 0f;
    private void Update()
    {
        timer -= Time.deltaTime;
        if(timer <= 0f)
        {
            Scratch();
        }
    }
    public void Scratch()
    {
        GameObject obj = Instantiate(scratch);
        Scratch sc = obj.GetComponent<Scratch>();
        sc.entity = this;
        sc.dmg = ItemManager.ConvertJToken<float>(ItemManager.datas["FragmentOfMonster"].value["val1"])[Mathf.Min(ItemManager.ConvertJToken<float>(data.value["val1"]).Length - 1, Lv - 1)];
        obj.transform.position = new(Random.Range(-2.5f, 2.5f), Random.Range(-4.5f, 4.5f));
        timer = ItemManager.ConvertJToken<float>(ItemManager.datas["FragmentOfMonster"].value["val0"])[Mathf.Min(ItemManager.ConvertJToken<float>(data.value["val0"]).Length - 1, Lv - 1)];
    }
}
