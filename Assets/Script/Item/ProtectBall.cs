using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtectBall : Item
{
    public GameObject protectBall;
    private float timer = 0;
    private int lv;
    public int Lv
    {
        get => lv;
        set
        {
            enabled = true;
            value = Mathf.Clamp(value, 1, data.val.Length);
            lv = value;
            timer = Mathf.Min(timer, data.val[value - 1]);
            GameManager.Inst.player.itemLevels["ProtectBall"] = value;
        }
    }
    public override void SetLv(int lv)
    {
        Lv = lv;
    }
    public override void AddLv()
    {
        Lv++;
    }
    public override void SubLv()
    {
        Lv--;
    }
    private void Update()
    {
        if (GameManager.Inst == null)
            return;
        if (!protectBall.activeSelf)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                timer = data.val[Lv-1];
                protectBall.SetActive(true);
                GameManager.Inst.player.Protect = true;
            }
        }
    }
}
