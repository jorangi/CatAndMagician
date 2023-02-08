using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public bool HitCheck;
    protected Rigidbody2D rigid;
    public bool PierceAble;
    protected Item _entity;
    public virtual Item entity
    {
        get => _entity;
        set
        {
            _entity = value;
            if(value.data.isBulletSpawner)
                spawner = value as BulletSpawner;
            if(spawner != null)
            {
                dmg = spawner.dmg;
                speed = spawner.speed;
            }
        }
    }
    public BulletSpawner spawner;
    public Transform Bullets;
    public List<Enemy> HitTarget = new();
    protected WaitForSeconds returnDelay = new WaitForSeconds(1f);
    public SpriteRenderer sprite;
    public Collider2D col;
    private AudioSource audioSource;
    public string _name;
    public bool rebuild;
    protected GameObject inPortal, outPortal;
    public int HitCount;
    public int MaxPierce;
    protected float speed;
    public float Speed
    {
        get => speed;
        set
        {
            speed = value;
        }
    }
    public float dmg;

    protected virtual void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        if (TryGetComponent(out col))
            col.isTrigger = true;
        col = GetComponent<Collider2D>();
        if(TryGetComponent(out sprite))
            sprite.sortingOrder = Random.Range(40, 51);
        audioSource = GetComponent<AudioSource>();
        HitTarget.Clear();
    }
    protected virtual void FixedUpdate()
    {
        rigid.MovePosition((Vector2)transform.position + Speed * Time.fixedDeltaTime * Vector2.up);
        if(transform.position.y > 7f)
        {
            ReturnObject();
        }
        else if(transform.position.y < -7f)
        {
            ReturnObject();
        }
    }
    protected virtual void OnEnable()
    {
        if (spawner != null)
        {
            dmg = spawner.dmg;
            speed = spawner.speed;
        }
    }
    protected virtual void HitEnemy()
    {
        for(int i = 0; i<HitTarget.Count; i++)
        {
            Knockback(HitTarget[i]);
            HitEnemy(HitTarget[i]);
            if(i >= MaxPierce)
            {
                break;
            }
        }
        HitTarget.Clear();
        HitCheck = false;
    }
    protected virtual void HitEnemy(Enemy enemy)
    {
        if(audioSource!=null)
            audioSource.Play();
        if (enemy == null)
            return;
        if(GameManager.Inst.player.itemLevels["LunaApple"] > 0)
        {
            dmg *= (1 + Time.fixedDeltaTime * Speed) * (1 + GameManager.Inst.player.itemLevels["LunaApple"] * 0.1f - 0.1f);
        }
        HitEnemy(dmg, enemy);
        if (GameManager.Inst.player.itemEvos["FrozenNametag"])
        {
            enemy.GetCC(_name.ToLower(), "slow", ItemManager.ConvertJToken<float>(ItemManager.datas["FrozenNametag"].value["val1"])[1] * 0.01f, ItemManager.ConvertJToken<float>(ItemManager.datas["FrozenNametag"].value["val1"])[0]);
        }
        if (GameManager.Inst.player.itemLevels["VenomBlackBirdOfParadisesClaw"] > 0)
        {
            VenomBlackBirdOfParadisesClaw VBBC = GameManager.Inst.player.Items["VenomBlackBirdOfParadisesClaw"] as VenomBlackBirdOfParadisesClaw;
            enemy.GetCC("VenomBlackBirdOfParadisesClaw", "poison", ItemManager.ConvertJToken<float>(VBBC.data.value["val0"])[Mathf.Min(ItemManager.ConvertJToken<float>(VBBC.data.value["val0"]).Length - 1, VBBC.Lv - 1)], ItemManager.ConvertJToken<float>(VBBC.data.value["val0"])[5]);
        }
    }
    protected virtual void HitEnemy(float dmg, Enemy enemy)
    {
        dmg *= GameManager.Inst.player.BulletDmgRatio + GameManager.Inst.player.NumericalBulletDmg;
        enemy.HP += enemy.ChangeHS(true, -dmg);

    }
    public virtual bool RebuildBullet()
    {
        if (!GameManager.Inst.player.itemEvos["AcceleratePortal"])
            return false;
        if (Random.Range(0f, 1f) <= ItemManager.ConvertJToken<float>(ItemManager.datas["AcceleratePortal"].value["val1"])[0] * 0.01f)
        {
            inPortal = Instantiate(GameManager.Inst.player.inPortal);
            inPortal.transform.position = new(transform.position.x, 4.6f);
            outPortal = Instantiate(GameManager.Inst.player.outPortal);
            outPortal.transform.position = new(transform.position.x, -4.5f);
            return true;
        }
        return false;
    }
    protected virtual void Knockback(Enemy enemy)
    {
        if (entity == null || !entity.data.value.ContainsKey("knockback"))
            return;
        float knockbackvalue = ItemManager.ConvertJToken<float>(entity.data.value["knockback"])[Mathf.Min(Mathf.Max(0, ItemManager.ConvertJToken<float>(entity.data.value["knockback"]).Length - 1), Mathf.Max(0, entity.Lv - 1))] + GameManager.Inst.player.Knockback;
        enemy.Knockbacked(knockbackvalue);
    }
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && !HitCheck)
        {
            HitTarget.Add(collision.transform.parent.GetComponent<Enemy>());
            HitEnemy();
            PierceCheck(PierceAble);
        }
        if(collision.CompareTag("Remove"))
        {
            ReturnObject();
        }
        if (collision.gameObject == inPortal && rebuild)
        {
            transform.position = outPortal.transform.position;
            Destroy(inPortal);
            Destroy(outPortal);
        }
    }
    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy"))
        {
            HitTarget.Remove(collision.transform.parent.GetComponent<Enemy>());
        }
    }
    protected virtual void PierceCheck(bool nonPierce)
    {
        if (MaxPierce > 0)
        {
            HitCount++;
            if (GameManager.Inst.player.Items["PiercingBullet"].Evo)
            {
                AddPiercingDmg();
            }
            MaxPierce--;
            HitCheck = false;
        }
        else if(!nonPierce)
        {
            ReturnObject();
            HitCheck = true;
        }
    }
    public void AddPiercingDmg()
    {
        dmg *= 1 + (HitCount * ItemManager.ConvertJToken<float>(ItemManager.datas["PiercingBullet"].value["val1"])[Mathf.Min(ItemManager.ConvertJToken<float>(ItemManager.datas["PiercingBullet"].value["val1"]).Length - 1, GameManager.Inst.player.itemLevels[entity.data.value["id"].ToString()] - 1)] * 0.01f);
    }
    public void ReturnObject()
    {
        if (inPortal != null)
            Destroy(inPortal);
        if (outPortal != null)
            Destroy(outPortal);
        if(gameObject.activeSelf)
            StartCoroutine(ReturnObj());
    }
    protected virtual IEnumerator ReturnObj()
    {
        HitTarget.Clear();
        if (sprite !=null)
            sprite.enabled = false;
        if(col != null)
            col.enabled = false;
        if(Bullets != null)
        {
            transform.SetParent(Bullets);
        }
        yield return returnDelay;
        gameObject.SetActive(false);
        if (sprite != null)
            sprite.enabled = true;
        if (col != null)
            col.enabled = true;
        if(Bullets == null)
            Destroy(gameObject, 1f);
    }
}
