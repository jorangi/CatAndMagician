using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BulletSpawner : MonoBehaviour
{
    public GameObject itemslot;
    public GameObject Bullets;
    public BulletData bulletData;
    public float dmg;
    public float delay;
    public float speed;
    private int lv;
    public int Lv
    {
        get => lv;
        set
        {
            enabled = true;
            value = Mathf.Clamp(value, 1, 5);
            lv = value;
            dmg = bulletData.dmg[Mathf.Min(value - 1, bulletData.dmg.Length-1)];
            delay = bulletData.delay[Mathf.Min(value - 1, bulletData.delay.Length - 1)];
            speed = bulletData.speed[Mathf.Min(value - 1, bulletData.speed.Length - 1)];
            LevelChanged();
            GameManager.Inst.player.itemLevels[bulletData.name] = value;
            switch (value)
            {
                case 1:
                    itemslot.GetComponentInChildren<TextMeshProUGUI>().text = "弘";
                    break;
                case 2:
                    itemslot.GetComponentInChildren<TextMeshProUGUI>().text = "弗";
                    break;
                case 3:
                    itemslot.GetComponentInChildren<TextMeshProUGUI>().text = "必";
                    break;
                case 4:
                    itemslot.GetComponentInChildren<TextMeshProUGUI>().text = "戊";
                    break;
                case 5:
                    itemslot.GetComponentInChildren<TextMeshProUGUI>().text = "打";
                    break;
            }
        }
    }
    private bool evo;
    public bool Evo
    {
        get => evo;
        set
        {
            evo = value;
            Evolved();
        }
    }
    public float timer;

    protected virtual void Awake()
    {
        Bullets = GameObject.FindGameObjectWithTag("Bullets");
        timer = 0.0f;

    }
    private void Start()
    {
        GameManager.Inst.player.spawners.Add(bulletData.name, this);
        PoolingBullet();
    }
    private void OnEnable()
    {

        if (itemslot == null)
        {
            itemslot = Instantiate(GameManager.Inst.player.itemSlot, GameManager.Inst.player.itemList);
            itemslot.name = bulletData.name;
            foreach(ItemData i in GameManager.Inst.player.itemDatas)
            {
                if(i.id==bulletData.name)
                {
                    itemslot.GetComponent<UnityEngine.UI.Image>().sprite = i.icon;
                }
            }
            switch (lv)
            {
                case 1:
                    itemslot.GetComponentInChildren<TextMeshProUGUI>().text = "弘";
                    break;
                case 2:
                    itemslot.GetComponentInChildren<TextMeshProUGUI>().text = "弗";
                    break;
                case 3:
                    itemslot.GetComponentInChildren<TextMeshProUGUI>().text = "必";
                    break;
                case 4:
                    itemslot.GetComponentInChildren<TextMeshProUGUI>().text = "戊";
                    break;
                case 5:
                    itemslot.GetComponentInChildren<TextMeshProUGUI>().text = "打";
                    break;
            }
        }
        itemslot.SetActive(true);
        if (Lv > 0)
        {
            dmg = bulletData.dmg[Lv - 1];
            delay = 1 / (bulletData.delay[Lv - 1] + GameManager.Inst.player.Delay);
            speed = bulletData.speed[Lv - 1] + GameManager.Inst.player.BulletSpeed;
        }
    }
    protected virtual void LevelChanged()
    {

    }
    protected virtual void Evolved()
    {

    }
    protected virtual void PoolingBullet()
    {
        GameObject parent = new();
        parent.transform.SetParent(Bullets.transform);
        parent.name = bulletData.name;

        for(int i = 0; i<bulletData.limit; i++)
        {
            GameObject obj = Instantiate(bulletData.bullet, parent.transform);
            obj.name = Bullets.name;
            obj.SetActive(false);
        }
    }
    protected virtual void Update()
    {
        if (lv == 0)
            return;
        timer += Time.deltaTime;
        if (timer >= (1 / (delay * GameManager.Inst.player.Delay)))
        {
            timer = 0;
            ShootBullet();
        }
    }
    protected virtual void ShootBullet()
    {
        GameObject obj = Bullets.transform.Find(bulletData.name).GetChild(0).gameObject;
        obj.transform.SetParent(null);
        Bullet bullet = obj.GetComponent<Bullet>();
        bullet.dmg = dmg * GameManager.Inst.player.BulletDmg;
        bullet.speed = Mathf.Max(0.1f, speed * GameManager.Inst.player.BulletSpeed);
        bullet.knockback = bulletData.knockback * GameManager.Inst.player.Knockback;
        obj.transform.localScale = new(GameManager.Inst.player.BulletSize, GameManager.Inst.player.BulletSize);
        obj.name = bulletData.name;
        obj.transform.position = transform.position;
        obj.SetActive(true);
    }
}
