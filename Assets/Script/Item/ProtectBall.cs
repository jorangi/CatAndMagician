using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtectBall : Item
{
    public GameObject protectBall;
    private float timer = 0;
    private void Update()
    {
        if (GameManager.Inst == null)
            return;
        if (!protectBall.activeSelf)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                timer = ItemManager.ConvertJToken<float>(data.value["val0"])[Mathf.Min(ItemManager.ConvertJToken<float>(data.value["val0"]).Length - 1, Lv - 1)];
                protectBall.SetActive(true);
                GameManager.Inst.player.Protect = true;
                if(Evo)
                {
                    GameManager.Inst.player.CCGuard++;
                }
            }
        }
    }
    protected override void LevelChanged()
    {
        base.LevelChanged();
        timer = Mathf.Min(timer, ItemManager.ConvertJToken<float>(data.value["val0"])[Mathf.Min(ItemManager.ConvertJToken<float>(data.value["val0"]).Length - 1, Lv - 1)]);
    }
}
