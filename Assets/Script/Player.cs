using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;
public class Player : Character
{
    //컴포넌트 관련
    public GameObject JewelryFlower;
    public VariableJoystick joystick;
    public GameObject blueStar;
    public GameObject inPortal, outPortal;
    [SerializeField]
    private ParticleSystem _flowerRain;
    public SpriteRenderer magicCirlce;
    public PauseMenu pauseMenu;
    public LevelupMenu levelupMenu;
    private TextMeshProUGUI levelText;
    private Image expBar;
    public Image stageBar;
    public TextMeshProUGUI stardustUI;
    public Transform itemList;
    public GameObject itemSlot;
    public StrIntDictionary itemLevels = new();
    public StrBoolDictionary itemEvos = new();
    public TextMeshProUGUI Timer;
    private Vector2 inputVec;

    //아이템 데이터
    public Dictionary<string, Item> Items = new();
    public List<ItemData> getItems;
    public ItemData StardustData;

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
            foreach(BulletSpawner spawner in FindObjectsOfType<BulletSpawner>())
            {
                spawner.Size = bulletSize;
            }
        }
    }

    //플레이어 이동 관련
    private bool isKeyboard;

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
    public float DelayRatio;
    public float NumericalDelay;

    //플레이어 투사체 속도
    public float BulletSpeedRatio;
    public float NumericalBulletSpeed;

    //플레이어 투사체 데미지
    public float BulletDmgRatio;
    public float NumericalBulletDmg;

    //플레이어 체력관련
    private int DelTearId = -1;
    private float shield;
    public float Shield
    {
        get => shield;
        set
        {
            value = Mathf.Max(value, 0.0f);
            if(RibbonShield > 0)
                RibbonShield -= Mathf.Max(0.0f, shield - value, RibbonShield);

            float d = Mathf.Max(shield - value - RibbonShield, 0.0f);

            if(DelTearId == -1)
            {
                Dictionary<int, float> t = Tears.OrderBy(item => item.Value).ToDictionary(x => x.Key, y => y.Value);
                for (int i = 0; i < t.Count; i++)
                {
                    int j = t.Keys.ToArray()[i];
                    float tempT = Tears[j];
                    Tears[j] -= Mathf.Min(Tears[j], d);
                    d = Mathf.Max(0, d - tempT);
                }
            }
            shield = value;
            RefreshHPBar();
            DelTearId = -1;
        }
    }
    private bool invincible;
    public bool Invincible
    {
        get => invincible;
        set
        {
            invincible = value;
        }
    }
    public float invincibleTime;
    public float playerHP;
    public float HP
    {
        get => hp;
        set
        {
            //체력 감소상황
            if(value < hp)
            {
                if(Protect)
                {
                    Protect = false;
                    return;
                }
                float t = Mathf.Max(hp - value - Shield, 0.0f); //감소값
                Shield -= ChangeHS(false, Mathf.Max(hp - value, 0.0f));
                value = hp - t;
            }
            else
            {
                value = Mathf.Clamp(value, 0.0f, maxhp);
            }
            if (itemEvos["CelestialStone"])
            {
                (Items["CelestialStone"] as CelestialStone).CelestialEvo();
            }
            if (itemEvos["GrapeBrooch"])
            {
                (Items["GrapeBrooch"] as GrapeBrooch).GrapeBroochEvo();
            }
            hp = Mathf.Clamp(value, 0.0f, maxhp);
            if (hp<=maxhp*0.1f && itemEvos["Churu"] && stageChuruEvo)
            {
                value += maxhp * 0.3f;
                hp = value;
                stageChuruEvo = false;
            }
            if (Shield == 0)
            {
                healthBar.localScale = new(10 * (hp / maxhp), 1);
                shieldBar.localScale = new(0, 1);
            }
            else
            {
                healthBar.localScale = new(value * 10 / Mathf.Max(value + Shield, maxhp), 1);
                shieldBar.localScale = new(Shield * 10 / Mathf.Max(value + Shield, maxhp), 1);
                shieldBar.localPosition = new(-0.5f + (healthBar.localScale.x / 10f), -0.65f);
            }
            if (hp==0)
            {
                if((Items["JewelryPomegranate"] as JewelryPomegranate).Resurrect > 0)
                {
                    HP += maxhp * ItemManager.ConvertJToken<float>(ItemManager.datas["JewelryPomegranate"].value["val1"])[1] * 0.01f;
                    (Items["JewelryPomegranate"] as JewelryPomegranate).Resurrect--;
                }
                else
                {
                    GameManager.Inst.stageManager.dead();
                }
            }
            if(hp == maxhp)
            {
                if(itemEvos["PumpkinPie"])
                {

                    (Items["PumpkinPie"] as PumpkinPie).FullHP = true;
                }
                if(itemEvos["BloodyDiamond"])
                {
                    (Items["BloodyDiamond"] as BloodyDiamond).FullHP = true;
                }
            }
            else
            {
                if (itemEvos["PumpkinPie"])
                {
                    (Items["PumpkinPie"] as PumpkinPie).FullHP = false;
                }
                if (itemEvos["BloodyDiamond"])
                {
                    (Items["BloodyDiamond"] as BloodyDiamond).FullHP = false;
                }
            }
        }
    }
    private float numericalmaxhp;
    public float NumericalMaxHP
    {
        get => numericalmaxhp;
        set
        {
            numericalmaxhp = value;
            ChangeMaxHP();
        }
    }
    private float maxhpratio = 1f;
    public float MaxHPRatio
    {
        get => maxhpratio;
        set
        {
            maxhpratio = value;
            ChangeMaxHP();
        }
    }
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

    //플레이어 체력재생관련
    public float Recover;

    //플레이어 상태이상 관련
    public float Drained = 1f;
    public bool Dizziness;
    public float DizzinessTimer;


    //플레이어 경험치 관련
    public int levelupCount;
    private int[] ExpIncrease = { 2, 4, 6, 8, 10};
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
            if (Items["InlaidOpalRing"].Evo)
            {
                GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
                if(enemies.Length > 0)
                {
                    enemies[UnityEngine.Random.Range(0, enemies.Length)].GetComponent<Enemy>().HP -= value;
                }
            }
            while (value >= maxExp)
            {
                if(itemEvos["FragmentBluestar"])
                {
                    GameObject bs = Instantiate(blueStar);
                    bs.GetComponent<BlueStar>().dmg = ItemManager.ConvertJToken<float>(ItemManager.datas["FragmentBluestar"].value["val1"])[0] * BulletDmgRatio;
                    bs.GetComponent<BlueStar>().Speed = 6 * BulletSpeedRatio;
                    bs.transform.localScale = new(bulletSize, bulletSize);
                    bs.transform.position = new(0, -4.5f);
                }
                value -= maxExp;
                Lv++;
                levelupCount++;
            }
            expBar.fillAmount = (float)value / maxExp;
            exp = value;
        }
    }

    //스타더스트 관련
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

    //플레이어 아이템 관련 전용 필드
    public int InlaidCount;
    public bool GoldCubeItems;
    private float ribbonShield;
    public float RibbonShield
    {
        get => ribbonShield;
        set
        {
            value = Mathf.Max(0.0f, value);
            ribbonShield = value;
        }
    }
    public Dictionary<int, float> Tears = new();
    public bool stageChuruEvo = true;
    private bool onTalisman;
    public bool OnTalisman
    {
        get => onTalisman;
        set
        {
            onTalisman = value;
            if(!value)
                StartCoroutine(OnTalismanTimer());
        }
    }
    private float talisman;
    public float Talisman
    {
        get => talisman;
        set
        {
            talisman = value;
        }
    }
    private WaitForSeconds TalismanTimer = new(1f);

    public bool FlowerShoesOn = false;
    private WaitForSeconds FlowerainT = new(1f);
    private Coroutine flowerain;
    public float flowerShoesDIMax;
    private float flowerShoesDITimer;
    private float flowerShoesDI;
    public float FlowerShoesDI
    {
        get => flowerShoesDI;
        set
        {
            BulletDmgRatio /= 1 + (flowerShoesDI * 0.01f);
            value = Mathf.Clamp(value, 0, flowerShoesDIMax);
            flowerShoesDI = value;
            BulletDmgRatio *= (1 + value * 0.01f);
            flowerShoesDITimer = 1f;
            if(value == flowerShoesDIMax && flowerain == null && FindObjectOfType<FlowerShoes>().Evo)
            {
                flowerain = StartCoroutine(Flowerain());
            }
        }
    }
    private int piercingTarget = 0;
    public int PiercingTarget
    {
        get => piercingTarget;
        set
        {
            piercingTarget = value;
        }
    }
    private bool ornamentMagicKnife = false;
    public bool OrnamentMagicKnife
    {
        get => ornamentMagicKnife;
        set
        {
            ornamentMagicKnife = value;
        }
    }
    public Bullet recentProjectile;
    private int projectile;
    public int Projectile
    {
        get => projectile;
        set
        {
            projectile = value;
            if(itemEvos["MagicHat"] && value % (int)ItemManager.ConvertJToken<float>(ItemManager.datas["MagicHat"].value["val1"])[1] == 0)
            {
                recentProjectile.dmg *= 1 + (ItemManager.ConvertJToken<float>(ItemManager.datas["MagicHat"].value["val1"])[2] * 0.01f);
            }
        }
    }
    private int hitInlaidProjectile = 0;
    public int HitInlaidProjectile
    {
        get => hitInlaidProjectile;
        set
        {
            if (!Items["InlaidDiamondRing"].Evo)
            {
                return;
            }
            hitInlaidProjectile = value;
            if (value >= ItemManager.ConvertJToken<int>(Items["InlaidDiamondRing"].data.value["val1"]) [0])
            {
                HitInlaidProjectile = 0;
                (Items["InlaidDiamondRing"] as InlaidDiamondRingSpawner).Laser.gameObject.SetActive(true);
                (Items["InlaidDiamondRing"] as InlaidDiamondRingSpawner).Laser.timer = ItemManager.ConvertJToken<float>(Items["InlaidDiamondRing"].data.value["val1"])[1];
            }
        }
    }

    protected override void Awake()
    {
        base.Awake();
        Canvas canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        expBar = canvas.transform.Find("exp").GetComponent<Image>();
        levelText = canvas.transform.Find("level").GetComponent<TextMeshProUGUI>();
        itemList = canvas.transform.Find("ItemList").transform.Find("list");
        stageBar = canvas.transform.Find("stageBar").GetComponent<Image>();
        Timer = stageBar.GetComponentInChildren<TextMeshProUGUI>();
        getItems = new()
        {
            Capacity = 12
        };
    }
    private void PlayerInit()
    {
        MoveSpeed = 5f;
        MoveSpeedRatio = 1.0f;
        playerHP = 100.0f;
        NumericalMaxHP = playerHP;
        MaxHPRatio = 1.0f;
        HP = maxhp;
        BulletDmgRatio = 1;
        DelayRatio = 1;
        Knockback = 1;
        BulletSpeedRatio = 1;
        invincibleTime = 0.7f;
        Recover = 1f;
        AddItem("VenomBlackBirdOfParadisesClaw");
        AddItem("VenomBlackBirdOfParadisesClaw");
        AddItem("VenomBlackBirdOfParadisesClaw");
        AddItem("VenomBlackBirdOfParadisesClaw");
        AddItem("VenomBlackBirdOfParadisesClaw");

        AddItem("TearOfTheSea");
        AddItem("TearOfTheSea");
        AddItem("TearOfTheSea");
        AddItem("TearOfTheSea");
        AddItem("TearOfTheSea");
        EvoItem("TearOfTheSea");
        InlaidItem("TearOfTheSea");

        AddItem("JewelryCuckoosEgg");
        AddItem("JewelryCuckoosEgg");
        AddItem("JewelryCuckoosEgg");
        AddItem("JewelryCuckoosEgg");
        AddItem("JewelryCuckoosEgg");
        EvoItem("JewelryCuckoosEgg");
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
    private void Update()
    {
        magicCirlce.transform.Rotate(0, 0, Time.deltaTime * 100);
        HP += ChangeHS(true, (Recover - 1) * Time.deltaTime);
        if(levelupCount>0)
        {
            levelupMenu.gameObject.SetActive(true);
        }
        if(flowerShoesDITimer>0)
        {
            flowerShoesDITimer -= Time.deltaTime;
        }
        else
        {
            flowerShoesDI = 0f;
        }
    }
    private void FixedUpdate()
    {
        if(!Dizziness)
        {
            if (joystick.transform.GetChild(0).gameObject.activeSelf)
                isKeyboard = false;
            if (!isKeyboard)
                inputVec = new(joystick.Horizontal, joystick.Vertical);
        }
        if (grab)
        {
            if ((grabbed - (Vector2)transform.position).sqrMagnitude > 0.01f)
            {
                Vector2 dir = grabbed - (Vector2)transform.position;
                transform.Translate(5f * Time.fixedDeltaTime * dir);
            }
            else
            {
                grab = false;
            }
        }
        else
        {
            if (stopped)
                return;

            if (Dizziness)
            {
                DizzinessTimer -= Time.fixedDeltaTime;
                if (DizzinessTimer <= 0f)
                {
                    inputVec = new(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f));
                    DizzinessTimer = UnityEngine.Random.Range(0.5f, 1f);
                }
            }

            Vector2 nextVec = MoveSpeed * MoveSpeedRatio *  Time.fixedDeltaTime * inputVec;
            if (FlowerShoesOn && GameManager.Inst.stageManager.gamePlaying && nextVec != Vector2.zero)
            {
                FlowerShoesDI += ItemManager.ConvertJToken<float>(ItemManager.datas["FlowerShoes"].value["val0"])[5] * 0.01f;
            }
            transform.position += (Vector3)nextVec;
        }
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -2.55f, 2.55f), Mathf.Clamp(transform.position.y, -3.0f, 4.2f), transform.position.z);
    }
    private void OnApplicationFocus(bool focus)
    {
        if(!focus && !levelupMenu.gameObject.activeSelf && GameManager.Inst.stageManager.gamePlaying)
        {
            pauseMenu.gameObject.SetActive(true);
        }
    }
    public void OnWASD(InputValue value)
    {
        isKeyboard = true;
        if(!Dizziness)
            inputVec = value.Get<Vector2>();
    }
    public void AddItem(string type)
    {
        ItemData d = Items[type].data;
        if (d.value["id"].ToString() == type && getItems.Contains(d))
        {
            if (d.isBulletSpawner)
            {
                var item = transform.Find("BulletSpawner").GetComponent($"{Type.GetType(type)}Spawner") as Item;
                item.AddLv();
            }
            else
            {
                var item = transform.Find("Items").GetComponent(Type.GetType(type)) as Item;
                item.AddLv();
            }
        }
        else if (d.value["id"].ToString() == type)
        {
            getItems.Add(d);
            if (d.isBulletSpawner)
            {
                var item = transform.Find("BulletSpawner").GetComponent($"{Type.GetType(type)}Spawner") as Item;
                item.itemSlot = Instantiate(itemSlot, itemList);
                item.itemSlot.name = type;
                item.itemSlot.GetComponent<Image>().sprite = item.data.icon;
                item.AddLv();
            }
            else
            {
                var item = transform.Find("Items").GetComponent(Type.GetType(type)) as Item;
                item.itemSlot = Instantiate(itemSlot, itemList);
                item.itemSlot.name = type;
                item.itemSlot.GetComponent<Image>().sprite = item.data.icon;
                item.AddLv();
            }
        }
        if(d.inlaidAble)
        {
            HammerForInlaid hammer = Items["HammerForInlaid"] as HammerForInlaid;
            if (hammer.InlaidCount <= hammer.inlaidMax)
            {
                hammer.InlaidCount++;
                InlaidCount++;
            }
        }
    }
    public void EvoItem(string type)
    {
        if (ItemManager.datas[type].isBulletSpawner)
        {
            var item = GetComponentInChildren(Type.GetType($"{type}Spawner")) as Item;
            item.Evo = true;
        }
        else
        {
            var item = transform.Find("Items").GetComponent(Type.GetType(type)) as Item;
            item.Evo = true;
        }
        itemEvos[type] = true;
    }
    public void InlaidItem(string type)
    {
        if (Items[type].Lv > 0)
        {
            Items[type].InlaidItem();
        }
    }
    public void SetLevelItem(string type, int level)
    {
        if(type == "")
        {

        }
        Item item = FindObjectOfType(Type.GetType(type)) as Item;
        item.SetLv(level);
    }
    public void Hit(Enemy enemy, float dmg)
    {
        StartCoroutine(InvincibleCoroutine(enemy, dmg));
    }
    public IEnumerator InvincibleCoroutine(Enemy enemy, float dmg)
    {
        if (Invincible)
            yield break;
        HP += ChangeHS(true, -dmg);
        if (itemEvos["SapphireEncrustedStatue"])
        {
            GameObject obj = Instantiate((Items["SapphireEncrustedStatue"] as SapphireEncrustedStatue).sapphireObj);
            obj.transform.position = transform.position;
        }
        if (itemEvos["BigRuby"] && UnityEngine.Random.Range(0f, 1f) <= ItemManager.ConvertJToken<float>(ItemManager.datas["SapphireEncrustedStatue"].value["val1"])[0] * 0.01f)
        {
            enemy.HP += enemy.ChangeHS(true, -dmg);
        }
        if (itemEvos["PurpleCharm"])
        {
            BulletDmgRatio *= 1 + ItemManager.ConvertJToken<float>(ItemManager.datas["PurpleCharm"].value["val1"])[0] * 0.01f;
        }
        Invincible = true;
        float t = invincibleTime;
        sprite.color = Color.gray;
        while (t > 0)
        {
            t -= Time.deltaTime;
            yield return null;
        }
        sprite.color = Color.white;
        Invincible = false;
        if (itemEvos["PurpleCharm"])
        {
            BulletDmgRatio /= 1 + ItemManager.ConvertJToken<float>(ItemManager.datas["PurpleCharm"].value["val1"])[0] * 0.01f;
        }
    }
    private IEnumerator OnTalismanTimer()
    {
        yield return TalismanTimer;
        OnTalisman = true;
    }
    private IEnumerator Flowerain()
    {
        _flowerRain.gameObject.SetActive(true);
        _flowerRain.Play();
        while(flowerShoesDI == flowerShoesDIMax)
        {
            foreach(GameObject _e in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                Enemy e = _e.GetComponent<Enemy>();
                e.HP += e.ChangeHS(true, -ItemManager.ConvertJToken<float>(ItemManager.datas["FlowerShoes"].value["val1"])[0] * BulletDmgRatio);
            }
            yield return FlowerainT;
        }
        flowerain = null;
        _flowerRain.Stop();
    }
    public void ChangeMaxHP()
    {
        float a = NumericalMaxHP * MaxHPRatio - maxhp;
        maxhp = NumericalMaxHP * MaxHPRatio;
        HP += a;
    }
    protected override IEnumerator Poison(CC cc)
    {
        WaitForSeconds onesec = new(1f);
        while (cc.time > 0)
        {
            HP -= (float)cc.value;
            yield return onesec;
        }
    }
    protected override IEnumerator Bleed(CC cc)
    {
        while (cc.time > 0)
        {
            Vector2 old = transform.position;
            yield return null;
            if (old != (Vector2)transform.position)
            {
                HP -= (float)cc.value * Time.deltaTime;
            }
        }
    }
    public void GetTear(float val, float timer)
    {
        StartCoroutine(TearOfTheSeaShield(val, timer));
    }
    //아이템 효과 혹은 상태이상 등으로 인한 체력 및 보호막 증감량 계산
    public float ChangeHS(bool isHP, float val)
    {
        if(isHP)
        {
            if(val > 0)
            {
                float d = 0f;
                foreach(CC c in CC)
                {
                    if(c.type == "Drained")
                    {
                        d += (float)c.value;
                    }
                }
                val *= Mathf.Clamp(1 - d, 0, 1);
            }
            else
            {
                val *= Vulnerable;
            }
        }
        else
        {
            if(val > 0)
            {
                if (itemEvos["TearOfTheSea"])
                {
                    val *= 1 + (ItemManager.ConvertJToken<float>(ItemManager.datas["TearOfTheSea"].value["val1"])[0] * 0.01f);
                }
            }
            else
            {
                
            }
        }
        return val;
    }
    public IEnumerator TearOfTheSeaShield(float val, float timer)
    {
        int r;
        do
        {
            r = UnityEngine.Random.Range(0, 1000);
        } while (Tears.ContainsKey(r));

        Tears.Add(r, ChangeHS(false, val * (1 + (ItemManager.ConvertJToken<float>(ItemManager.datas["TearOfTheSea"].value["val1"])[0] * 0.01f))));
        Shield += Tears[r];

        while(timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }
        DelTearId = r;
        Shield += -Tears[r];
        Tears.Remove(r);
        RefreshHPBar();
    }
    protected override void RefreshHPBar()
    {
        base.RefreshHPBar();
        if (Shield == 0)
        {
            healthBar.localScale = new(10 * (HP / maxhp), 1);
            shieldBar.localScale = new(0, 1);
        }
        else
        {
            healthBar.localScale = new(HP * 10 / Mathf.Max(HP + Shield, maxhp), 1);
            shieldBar.localScale = new(Shield * 10 / Mathf.Max(HP + Shield, maxhp), 1);
            shieldBar.localPosition = new(-0.5f + (healthBar.localScale.x / 10f), -0.65f);
        }
    }
    public void KillEnemy()
    {
        if (itemLevels["LeadCube"] > 0)
        {
            LeadCube leadCube = Items["LeadCube"] as LeadCube;
            if (leadCube.Evo)
            {
                leadCube.KillCount+= ItemManager.ConvertJToken<int>(Items["LeadCube"].data.value["val1"])[0];
            }
            else
            {
                leadCube.KillCount += ItemManager.ConvertJToken<int>(Items["LeadCube"].data.value["val0"])[1];
            }
        }
        if (itemLevels["GoldCube"] > 0)
        {
            GoldCube goldCube = Items["GoldCube"] as GoldCube;
            if (goldCube.Evo)
            {
                goldCube.Gold += UnityEngine.Random.Range(ItemManager.ConvertJToken<int>(goldCube.data.value["val1"])[0], ItemManager.ConvertJToken<int>(goldCube.data.value["val1"])[1] + 1);
            }
        }
    }
}