using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class Player : MonoBehaviour
{
    //컴포넌트 관련
    [SerializeField]
    private SpriteRenderer magicCirlce;
    public PauseMenu pauseMenu;
    public LevelupMenu levelupMenu;
    public PlayerInputs input;
    private Transform healthBar;
    private TextMeshProUGUI levelText;
    private Image expBar;
    public Image stageBar;
    public TextMeshProUGUI stardustUI;
    public Transform itemList;
    public GameObject itemSlot;
    public StrBspawnerDictionary spawners = new();
    public StrBdataDictionary bulletDatas = new();
    public StrIntDictionary itemLevels = new();
    public TextMeshProUGUI Timer;

    //아이템 데이터
    public ItemData[] itemDatas;
    public List<ItemData> getItems;
    public ItemData StardustData;
    public int levelupCount;

    //플레이어 투사체 넉백 관련
    private float knockback;
    public float Knockback
    {
        get => knockback;
        set
        {
            knockback = value;
        }
    }

    //플레이어 투사체 크기 관련
    private float bulletSize = 1.0f;
    public float BulletSize
    {
        get => bulletSize;
        set
        {
            bulletSize = value;
        }
    }

    //플레이어 피격 관련
    private float spikeDmg;
    public float SpikeDmg
    {
        get => spikeDmg;
        set
        {
            spikeDmg = value;
        }
    }

    //플레이어 공격속도
    private float delay = 0;
    public float Delay
    {
        get => delay;
        set
        {
            delay = value;
        }
    }

    //플레이어 투사체 속도
    private float bulletSpeed = 0;
    public float BulletSpeed
    {
        get => bulletSpeed;
        set
        {
            bulletSpeed = value;
        }
    }

    //플레이어 투사체 데미지
    private float bulletDmg = 0;
    public float BulletDmg
    {
        get => bulletDmg;
        set
        {
            bulletDmg = value;
        }
    }

    //플레이어 체력관련
    private bool invincible;
    public bool Invincible
    {
        get => invincible;
        set
        {
            invincible = value;
            if(invincible)
            {
                StartCoroutine(Hit());
            }
        }
    }
    public float invincibleTime;
    private float hp;
    public float maxhp;
    public float recover;
    private bool protect;
    public bool Protect
    {
        get => protect;
        set
        {
            if(protect)
            {
                transform.Find("ProtectBall").gameObject.SetActive(false);
                Invincible = true;
            }
            protect = value;
        }
    }
    public float HP
    {
        get => hp;
        set
        {
            if(value < hp)
            {
                Invincible = true;
                if(Protect)
                {
                    Protect = false;
                    return;
                }
            }
            value = Mathf.Clamp(value, 0.0f, maxhp);
            hp = value;
            if(hp==0)
            {
                GameManager.Inst.stageManager.dead();
            }
            healthBar.localScale = new (10 * (hp/maxhp), 1);
        }
    }

    //플레이어 경험치 관련
    private int[] ExpIncrease = { 1, 2, 3, 5, 9};
    private int lv;
    public int Lv
    {
        get => lv;
        set
        {
            lv = value;
            levelText.text = $"LV. {value}";
            if (value == 1)
                return;
            maxExp += ExpIncrease[Mathf.Min(value/20, 4)];
        }
    }
    public int maxExp;
    private int exp;
    public int Exp
    {
        get => exp;
        set
        {
            while (value >= maxExp)
            {
                value -= maxExp;
                Lv++;
                levelupCount++;
            }
            expBar.fillAmount = (float)value / maxExp;
            exp = value;
        }
    }
    private int stardust;
    public int StarDust
    {
        get => stardust;
        set
        {
            stardust = value;
            stardustUI.text = value.ToString();
        }
    }

    private void Awake()
    {
        Canvas canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        healthBar = transform.Find("HealthBar");
        expBar = canvas.transform.Find("exp").GetComponent<Image>();
        levelText = canvas.transform.Find("level").GetComponent<TextMeshProUGUI>();
        itemList = canvas.transform.Find("ItemList").transform.Find("list");
        stageBar = canvas.transform.Find("stageBar").GetComponent<Image>();
        Timer = stageBar.GetComponentInChildren<TextMeshProUGUI>();
        input = new PlayerInputs();
        getItems.Capacity = 12;
    }
    private void PlayerInit()
    {
        maxhp = 100.0f;
        HP = maxhp;
        BulletDmg = 1;
        Delay = 1;
        Knockback = 1;
        BulletSpeed = 1;
        invincibleTime = 0.7f;
        AddItem("CatsEye");
    }
    private void Start()
    {
        PlayerInit();
        maxExp = 5;
        exp = 0;
        Exp = 0;
        Lv = 1;
        levelText.text = $"LV. {Lv}";
    }
    private void OnEnable()
    {
        input.Enable();
        input.Player.Drag.performed += Drag;
    }
    private void OnDisable()
    {
        input.Player.Drag.performed -= Drag;
        input.Disable();
    }
    private void Update()
    {
        magicCirlce.transform.Rotate(0, 0, Time.deltaTime * 100);
        HP += recover*Time.deltaTime;
        if(levelupCount>0)
        {
            levelupMenu.gameObject.SetActive(true);
        }
    }
    private void OnApplicationFocus(bool focus)
    {
        if(!focus && !levelupMenu.gameObject.activeSelf && GameManager.Inst.stageManager.gamePlaying)
        {
            pauseMenu.gameObject.SetActive(true);
        }
    }
    public void Drag(InputAction.CallbackContext obj)
    {
        transform.Translate(obj.ReadValue<Vector2>().x / 150.0f, obj.ReadValue<Vector2>().y / 150.0f, 0);
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -2.55f, 2.55f), Mathf.Clamp(transform.position.y, -3.0f, 4.67f), transform.position.z);
    }
    public void AddItem(string type)
    {
        string[] weapon = { "CatsEye", "Rosruc", "MagicBall", "MagicArrow", "Airstrike"};
        foreach(ItemData d in itemDatas)
        {
            if(d.id == type && !getItems.Contains(d))
            {
                getItems.Add(d);
            }
        }
        if (Array.IndexOf(weapon, type)>-1)
        {
            var item = GetComponentInChildren(Type.GetType($"{type}Spawner")) as BulletSpawner;
            item.enabled = true;
            item.Lv++;
        }
        else
        {
            var item = transform.Find("Items").GetComponent(Type.GetType(type)) as Item;

            if(item.itemSlot==null)
            {
                item.itemSlot = Instantiate(GameManager.Inst.player.itemSlot, GameManager.Inst.player.itemList); 
                foreach (ItemData i in itemDatas)
                {
                    if (i.id == type)
                    {
                        item.itemSlot.GetComponent<Image>().sprite = i.icon;
                    }
                }
            }
            
            item.itemSlot.name = type;
            item.enabled = true;
            item.AddLv();

            switch (itemLevels[type])
            {
                case 1:
                    item.itemSlot.GetComponentInChildren<TextMeshProUGUI>().text = "Ⅰ";
                    break;
                case 2:
                    item.itemSlot.GetComponentInChildren<TextMeshProUGUI>().text = "Ⅱ";
                    break;
                case 3:
                    item.itemSlot.GetComponentInChildren<TextMeshProUGUI>().text = "Ⅲ";
                    break;
                case 4:
                    item.itemSlot.GetComponentInChildren<TextMeshProUGUI>().text = "Ⅳ";
                    break;
                case 5:
                    item.itemSlot.GetComponentInChildren<TextMeshProUGUI>().text = "Ⅴ";
                    break;
            }
        }
    }
    public void SetLevelItem(string type, int level)
    {
        if(transform.Find("BulletSpawner").gameObject.GetComponentInChildren(Type.GetType($"{type}Spawner")) != null)
        {
            BulletSpawner item = transform.Find("BulletSpawner").gameObject.GetComponentInChildren(Type.GetType($"{type}Spawner")) as BulletSpawner;
            item.Lv = level;
        }
        else
        {
            Item item = transform.Find("Items").GetComponent(Type.GetType(type)) as Item;
            item.SetLv(level);
        }
    }
    private IEnumerator Hit()
    {
        float t = invincibleTime;
        GetComponent<SpriteRenderer>().color = Color.gray;
        while(t>0)
        {
            t -= Time.deltaTime;
            yield return null;
        }
        GetComponent<SpriteRenderer>().color = Color.white;
        Invincible = false;
    }
}