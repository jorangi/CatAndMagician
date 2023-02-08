using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HamsterWheel : Item
{
    public GameObject hamsterWheel;
    private float dmg;
    private List<float> posY = new();
    private void FixedUpdate()
    {
        posY.Add(transform.position.y);
        if(posY.Count > 1 && posY[^1] <= posY[^2])
        {
            posY.Clear();
        }
        if(posY.Count >= 20)
        {
            GameObject obj = Instantiate(hamsterWheel);
            obj.transform.position = transform.position;
            obj.GetComponent<HamWheel>().dmg = dmg;
            posY.Clear();
        }
    }
    protected override void LevelChanged()
    {
        base.LevelChanged();
        dmg = ItemManager.ConvertJToken<float>(data.value["val0"])[Mathf.Min(ItemManager.ConvertJToken<float>(data.value["val0"]).Length - 1, Lv - 1)];
    }
}
