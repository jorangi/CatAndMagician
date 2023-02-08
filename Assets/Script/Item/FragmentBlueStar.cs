using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FragmentBluestar : Item
{
    public CircleCollider2D magnet;
    protected override void LevelChanged()
    {
        base.LevelChanged();
        magnet.gameObject.SetActive(true);
        if(Lv == 1)
        {
            magnet.radius = ItemManager.ConvertJToken<float>(data.value["val0"])[0] * 0.1f;
        }
        else
        {
            magnet.radius *= 1 + ItemManager.ConvertJToken<float>(data.value["val0"])[Mathf.Min(ItemManager.ConvertJToken<float>(data.value["val0"]).Length - 1, Lv - 1)] * 0.01f;
        }
    }
}
