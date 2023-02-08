using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    private WaitForSeconds onesec = new(1f);
    protected Collider2D triggerCol;
    [SerializeField]
    protected Sprite CautionBox;

    private float knockbackResult = 0f;
    private float KnockbackResult
    {
        get => knockbackResult;
        set
        {
            knockbackResult = value;
            if (GameManager.Inst.player.itemEvos["EmeraldRabbitsTail"] && value > ItemManager.ConvertJToken<float>(ItemManager.datas["EmeraldRabbitsTail"].value["val1"])[0])
            {
                GetCC("EmeraldRabbitsTail", "vulnerable", ItemManager.ConvertJToken<float>(ItemManager.datas["EmeraldRabbitsTail"].value["val1"])[1] * 0.01f, 99999);
            }
        }
    }
    public float knockbackResist = 0f;
    private float knockback = 0f;
    public bool inScreen = false;
    public bool NearbyPlayer;
    private bool spikeTimer;
    protected bool SpikeTimer
    {
        get => spikeTimer;
        set
        {
            if (value && gameObject.activeSelf)
            {
                StartCoroutine(SpikeTimerOn());
            }
            spikeTimer = value;
        }
    }
    public EnemyData data;
    public float dmg;
    public virtual float HP
    {
        get => hp;
        set
        {
            if (hp == 0)
            {
                return;
            }
            //체력 감소상황
            if (value < hp)
            {
                float t = Mathf.Max(hp - value - Shield, 0.0f); //감소값
                Shield -= Mathf.Max(hp - value, 0.0f);
                value = hp - t;
            }
            else
            {
                value = Mathf.Clamp(value, 0.0f, maxhp);
            }
            hp = Mathf.Clamp(value, 0.0f, maxhp);
            RefreshHPBar();
            if (hp == 0)
            {
                if (GameManager.Inst.player.itemLevels["JewelryFlower"] > 0 && Random.Range(0f, 1f) < ItemManager.ConvertJToken<float>(GameManager.Inst.player.Items["JewelryFlower"].data.value["val0"])[5] * 0.01f)
                    SpawnJewelryFlower();
                triggerCol.enabled = false;
                enabled = false;
                string star = "";
                switch (data.expLv)
                {
                    case 1:
                        star = "yellowexpstar";
                        break;
                    case 2:
                        star = "blueexpstar";
                        break;
                    case 3:
                        star = "greenexpstar";
                        break;
                    case 4:
                        star = "redexpstar";
                        break;
                    case 5:
                        star = "whiteexpstar";
                        break;
                    default:
                        return;
                }
                if (GameManager.Inst.player.itemEvos["JewelryCuckoosEgg"] && Random.Range(0f, 1f) <= ItemManager.ConvertJToken<float>(ItemManager.datas["JewelryCuckoosEgg"].value["val1"])[0] * 0.01f)
                {
                    GameObject item = Instantiate(GameManager.Inst.Drops["whiteexpstar"]);
                    item.name = star;
                    item.transform.position = transform.position;
                }
                else
                {
                    GameObject item = Instantiate(GameManager.Inst.Drops[star]);
                    item.name = star;
                    item.transform.position = transform.position;
                }
                GameManager.Inst.player.KillEnemy();
                StartCoroutine(Dead());
            }
        }
    }
    private float shield;
    public float Shield
    {
        get => shield;
        set
        {
            value = Mathf.Max(value, 0.0f);
            healthBar.localScale = new(HP * 10 / Mathf.Max(HP + value, maxhp), 1);
            shieldBar.localScale = new(value * 10 / Mathf.Max(HP + value, maxhp), 1);
            shieldBar.localPosition = new(-0.5f + (healthBar.localScale.x / 10f), -0.65f);
            shield = value;
        }
    }

    private void SpawnJewelryFlower()
    {
        float a = 1 + ItemManager.ConvertJToken<float>(GameManager.Inst.player.Items["InlaidPeridotRing"].data.value["val0"])[0] * 0.01f;
        GameObject flower = Instantiate((GameManager.Inst.player.Items["JewelryFlower"] as JewelryFlower).Flower);
        flower.transform.position = transform.position;
        if (GameManager.Inst.player.itemLevels["JewelryFlower"] > 0)
        {
            flower.transform.localScale = new(1, 1);
        }
        else
        {
            flower.transform.localScale = new(a, a);
        }
    }
    protected override void Awake()
    {
        base.Awake();
        triggerCol = transform.Find("Trigger").GetComponent<Collider2D>();
        col = transform.Find("Collider").GetComponent<Collider2D>();
        sprite = GetComponent<SpriteRenderer>();
        maxhp = data.hp;
        hp = data.hp;
        HP = data.hp;
        dmg = data.dmg;
        MoveSpeed = data.spd;
        knockbackResist = data.knockbackResist;
        Resist = data.ccResist;
        Shield = data.shield;
    }
    protected virtual void FixedUpdate()
    {
        if (grab)
        {
            if ((grabbed - (Vector2)transform.position).sqrMagnitude > 0.01f)
            {
                Vector2 dir = grabbed - (Vector2)transform.position;
                transform.Translate(5 * Time.fixedDeltaTime * dir.normalized);
            }
            else
            {
                grab = false;
            }
        }
        else
        {
            if (!stopped)
            {
                transform.Translate((MoveSpeed - knockback) * Time.fixedDeltaTime * Vector2.down);
            }
            else
                transform.Translate(-knockback * Time.fixedDeltaTime * Vector2.down);
            if (knockback > 0 && transform.position.y > 5f)
            {
                transform.position = new(transform.position.x, 5f);
                if (GameManager.Inst.player.itemEvos["Talisman"])
                {
                    GetCC("talisman", "stun", 0, ItemManager.ConvertJToken<float>(ItemManager.datas["talisman"].value["val1"])[0]);
                }
            }
            if (GameManager.Inst.player.itemEvos["ABoxOfMemories"] && transform.position.y < -5f && Random.Range(0f, 1f) <= ItemManager.ConvertJToken<float>(ItemManager.datas["ABoxOfMemories"].value["val1"])[0] * 0.01f)
            {
                transform.position = new(transform.position.x, 5f);
            }
            knockback = 0f;
        }
    }
    private void LateUpdate()
    {
        Vector3 pos = transform.position;
        transform.position = new (pos.x, pos.y, pos.y * Time.deltaTime);
        if(pos.z == transform.position.z)
        {
            transform.Translate(0, 0, -Time.deltaTime);
        }
    }
    public void Knockbacked(float force)
    {
        if(NearbyPlayer && GameManager.Inst.player.itemEvos["Trampoline"])
        {
            knockback += (1 - knockbackResist) * force * 2;
            KnockbackResult += knockback;
            return;
        }
        knockback += (1 - knockbackResist) * force;
        KnockbackResult += knockback;
    }
    private IEnumerator SpikeTimerOn()
    {
        yield return new WaitForSeconds(GameManager.Inst.player.invincibleTime);
        SpikeTimer = false;
    }
    protected virtual void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Remove"))
        {
            StartCoroutine(Dead());
        }
        else if (collision.CompareTag("Player"))
        {
            if (GameManager.Inst.player.OnTalisman)
            {
                float tr = GameManager.Inst.player.itemEvos["Trampoline"] ? (1 + ItemManager.ConvertJToken<float>(GameManager.Inst.player.Items["Trampoline"].data.value["val1"])[0] * 0.01f) : 1.0f;
                Knockbacked(GameManager.Inst.player.Talisman * tr);
            }
            if (!SpikeTimer)
            {
                SpikeTimer = true;
                HP += ChangeHS(true, GameManager.Inst.player.SpikeDmg);
                if (GameManager.Inst.player.itemEvos["SpikyBall"])
                {
                    GameObject obj = Instantiate((GameManager.Inst.player.Items["SpikyBall"] as SpikyBall).spikes);
                    obj.transform.position = transform.position;
                }
            }
            if(gameObject.activeSelf)
                GameManager.Inst.player.Hit(this, dmg);
        }
    }
    protected override IEnumerator Poison(CC cc)
    {
        Item item = GameManager.Inst.player.Items["VenomBlackBirdOfParadisesClaw"];
        WaitForSeconds onesec = new(1f);
        while (cc.time > 0)
        {
            if (item.Evo && ItemManager.ConvertJToken<float>(item.data.value["val1"])[0] <= CCAccum["poison"])
                HP -= (float)cc.value * (1 + ItemManager.ConvertJToken<float>(item.data.value["val1"])[1] * 0.01f);
            else
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
    private IEnumerator Dead()
    {
        GetComponentInChildren<BlackWingMark>()?.RemoveMark();
        triggerCol.enabled = false;
        col.enabled = false;
        sprite.enabled = false;
        MicsEffect.gameObject.SetActive(false);
        yield return onesec;
        gameObject.SetActive(false);
        Destroy(gameObject);
    }
    public float ChangeHS(bool isHP, float val)
    {
        if (isHP)
        {
            if (val > 0)
            {
            }
            else
            {
                val *= Vulnerable;
            }
        }
        else
        {
            if (val > 0)
            {
            }
            else
            {

            }
        }
        return val;
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
}
