using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeadCube : Item
{
    private int killCount = 0;
    public int KillCount
    {
        get => killCount;
        set
        {
            value = Mathf.Clamp(value, 0, 100);
            GameManager.Inst.player.NumericalMaxHP += (value - killCount) * 0.5f;
            killCount = value;
            if(value == 100)
            {
                foreach(ItemData item in GameManager.Inst.player.getItems)
                {
                    if (item.value["id"].ToString() == "LeadCube")
                    {
                        GameManager.Inst.player.Items["LeadCube"].itemSlot.SetActive(false);
                        GameManager.Inst.player.getItems.Remove(item);
                        break;
                    }
                }
                for(int i = 0; i<Lv; i++)
                {
                    GameManager.Inst.player.AddItem("GoldCube");
                }
            }
        }
    }
}
