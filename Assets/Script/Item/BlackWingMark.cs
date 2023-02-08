using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackWingMark : MonoBehaviour
{
    public SpriteRenderer sprite;
    public Sprite[] marks;
    private int count;
    public int Count
    {
        get => count;
        set
        {
            value = Mathf.Clamp(value, 0, (int)ItemManager.ConvertJToken<float>(GameManager.Inst.player.Items["BlackFeather"].data.value["val0"])[0]);
            GameManager.Inst.player.DelayRatio /= 1 + (ItemManager.ConvertJToken<float>(GameManager.Inst.player.Items["BlackFeather"].data.value["val0"])[1] * 0.01f * count);
            count = value;
            if (value == 0)
                sprite.sprite = null;
            else
            {
                sprite.sprite = marks[Mathf.Min(value - 1, (int)ItemManager.ConvertJToken<float>(GameManager.Inst.player.Items["BlackFeather"].data.value["val0"])[0] - 1)];
            }
            GameManager.Inst.player.DelayRatio *= 1 + (ItemManager.ConvertJToken<float>(GameManager.Inst.player.Items["BlackFeather"].data.value["val0"])[1] * 0.01f * value);
        }
    }
    public void RemoveMark()
    {
        Count = 0;
        Destroy(gameObject);
    }
}
