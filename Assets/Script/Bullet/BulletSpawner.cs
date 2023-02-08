using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class BulletSpawner : Item
{
    private float size = 1f;
    public float Size
    {
        get => size;
        set
        {
            size = value;
            ChangeSize();
        }
    }

    public GameObject bullet;
    public GameObject Bullets;
    public float dmg;
    public float delay;
    public float speed;
    public float timer;
    public float CurrentDelay;

    protected override void Awake()
    {
        base.Awake();
        Bullets = GameObject.FindGameObjectWithTag("Bullets");
        timer = 0.0f;
    }
    private void Start()
    {
        if (name != "BulletSpawner")
            return;
        PoolingBullet();
    }
    protected virtual void Update()
    {
        if (Lv == 0 && name == "BulletSpawner" || GameManager.Inst.player.stopped)
            return;
        timer += Time.deltaTime;
        if (name=="BulletSpawner")
        {
            CurrentDelay = (1 / (delay * GameManager.Inst.player.DelayRatio + GameManager.Inst.player.NumericalDelay));
            if (timer >= CurrentDelay)
            {
                timer = 0;
                ShootBullet();
            }
        }
        else
        {
            if (timer >= (1 / delay))
            {
                timer = 0;
                ShootBullet();
            }
        }
    }
    protected virtual void PoolingBullet()
    {
        if(Bullets.transform.Find(data.value["id"].ToString()))
        {
            return;
        }
        GameObject parent = new();
        parent.transform.SetParent(Bullets.transform);
        parent.name = data.value["id"].ToString();

        for(int i = 0; i<(int)data.value["limit"]; i++)
        {
            GameObject obj = Instantiate(bullet, parent.transform);
            obj.name = data.value["id"].ToString();
            obj.GetComponent<Bullet>().entity = this;
            obj.SetActive(false);
        }
    }
    protected virtual void ShootBullet()
    {
        GameObject obj = Bullets.transform.Find(data.value["id"].ToString()).GetChild(0).gameObject;
        ShootBullet(obj.GetComponent<Bullet>(), obj.transform.parent, data.value["id"].ToString());
    }
    protected virtual void ShootBullet(Bullet bullet, Transform parent, string name)
    {
        GameObject obj = bullet.gameObject;
        bullet.Bullets = parent;
        obj.transform.SetParent(null);
        bullet.MaxPierce = GameManager.Inst.player.PiercingTarget;
        obj.transform.localScale = new(Size, Size);
        obj.name = name;
        obj.transform.position = transform.position;
        bullet.rebuild = bullet.RebuildBullet();
        GameManager.Inst.player.recentProjectile = bullet;
        GameManager.Inst.player.Projectile++;
        obj.SetActive(true);
    }
    protected virtual void ChangeSize()
    {
    }
    protected override void LevelChanged()
    {
        base.LevelChanged();
        dmg = ItemManager.ConvertJToken<float>(data.value["dmg"])[Mathf.Min(Lv - 1, ItemManager.ConvertJToken<float>(data.value["dmg"]).Length - 1)];
        delay = ItemManager.ConvertJToken<float>(data.value["delay"])[Mathf.Min(Lv - 1, ItemManager.ConvertJToken<float>(data.value["delay"]).Length - 1)];
        speed = ItemManager.ConvertJToken<float>(data.value["speed"])[Mathf.Min(Lv - 1, ItemManager.ConvertJToken<float>(data.value["speed"]).Length - 1)];

        switch (Lv)
        {
            case 1:
                itemSlot.GetComponentInChildren<TextMeshProUGUI>().text = "¥°";
                break;
            case 2:
                itemSlot.GetComponentInChildren<TextMeshProUGUI>().text = "¥±";
                break;
            case 3:
                itemSlot.GetComponentInChildren<TextMeshProUGUI>().text = "¥²";
                break;
            case 4:
                itemSlot.GetComponentInChildren<TextMeshProUGUI>().text = "¥³";
                break;
            case 5:
                itemSlot.GetComponentInChildren<TextMeshProUGUI>().text = "¥´";
                break;
        }
    }
}
