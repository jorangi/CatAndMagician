using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Item : MonoBehaviour
{
    public ItemData data = new();
    private bool evo;
    public bool Evo
    {
        get => evo;
        set
        {
            evo = value;
            itemSlot.GetComponentInChildren<TextMeshProUGUI>().text = "★";
            Evolved();
        }
    }
    private int lv;
    public int Lv
    {
        get => lv;
        set
        {
            value = Mathf.Clamp(value, 1, 5);
            lv = value;
            LevelChanged();
        }
    }
    public GameObject itemSlot;
    protected virtual void Awake()
    {
        data = ItemManager.datas[GetType().ToString().Replace("Spawner", "")];
        if (data.value.ContainsKey("dmg"))
        {
            data.isBulletSpawner = true;
        }
        if(data.value.ContainsKey("inlaid"))
        {
            data.inlaidAble = true;
        }
        if(data.isBulletSpawner && name != "BulletSpawner")
        {
            return;
        }
        GameManager.Inst.player.Items.Add(GetType().ToString().Replace("Spawner", ""), this);
    }
    private void Start()
    {
    }
    protected T CovertJToken<T>(Newtonsoft.Json.Linq.JToken value)
    {
        List<T> values = new();
        int i = 0;
        foreach (Newtonsoft.Json.Linq.JToken val in value)
        {
            values.Add((T)System.Convert.ChangeType(val, typeof(T)));
            i++;
        }
        return values[0];
    }
    protected T[] ConvertJToken<T>(Newtonsoft.Json.Linq.JToken value)
    {
        List<T> values = new();
        int i = 0;
        foreach(Newtonsoft.Json.Linq.JToken val in value)
        {
            values.Add((T)System.Convert.ChangeType(val, typeof(T)));
            i++;
        }
        return values.ToArray();
    }
    public virtual void SetLv(int lv)
    {
        Lv = lv;
    }
    public virtual void AddLv()
    {
        Lv++;
    }
    public virtual void SubLv()
    {
        Lv--;
    }
    protected virtual void Evolved()
    {
    }
    protected virtual void LevelChanged()
    {
        enabled = true;
        GameManager.Inst.player.itemLevels[data.value["id"].ToString()] = Lv;
        TextMeshProUGUI t = itemSlot.GetComponentInChildren<TextMeshProUGUI>();
        switch (Lv)
        {
            case 1:
                t.text = "Ⅰ";
                break;
            case 2:
                t.text = "Ⅱ";
                break;
            case 3:
                t.text = "Ⅲ";
                break;
            case 4:
                t.text = "Ⅳ";
                break;
            case 5:
                t.text = "Ⅴ";
                break;
        }
    }
    public void InlaidItem()
    {
        if(data.inlaidAble)
            Inlaided();
    }
    protected virtual void Inlaided()
    {
        for(int i = GameManager.Inst.player.getItems.Count - 1; i>=0; i--)
        {
            GameManager.Inst.player.getItems.Remove(data);
        }
        itemSlot.SetActive(false);
        for(int i = 0; i < Lv; i++)
        {
            GameManager.Inst.player.AddItem(data.value["inlaid"].ToString());
        }
        if(Evo)
        {
            GameManager.Inst.player.EvoItem(data.value["inlaid"].ToString());
        }
    }
}
