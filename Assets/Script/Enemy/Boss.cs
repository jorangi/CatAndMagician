using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField]
    protected Sprite CautionBox;

    protected Coroutine idle;
    private bool spikeTimer;
    private bool SpikeTimer
    {
        get => spikeTimer;
        set
        {
            if (value)
            {
                StartCoroutine(SpikeTimerOn());
            }
            spikeTimer = value;
        }
    }
    public string Drops;
    public EnemyData data;
    public float dmg;
    private Transform healthBar;
    public float maxhp;
    protected float hp;
    public float spd;
    public float HP
    {
        get => hp;
        set
        {
            if (hp == 0)
            {
                return;
            }
            value = Mathf.Clamp(value, 0, maxhp);
            hp = value;
            healthBar.localScale = new(50 * (hp / maxhp), 1);
            if (hp == 0)
            {
                GetComponent<BoxCollider2D>().enabled = false;
                GameObject item = Instantiate(GameManager.Inst.Drops[Drops]);
                item.name = Drops;
                item.transform.position = transform.position;
                Destroy(gameObject);
                GameManager.Inst.stageManager.StageClear();
            }
        }
    }

    protected virtual void Awake()
    {
        healthBar = transform.Find("HealthBar");
        healthBar.SetParent(null);
        healthBar.position = new(-2.5f, 4.5f);

        maxhp = data.hp;
        hp = data.hp;
        HP = data.hp;
        dmg = data.dmg;
        spd = data.spd;
        StartCoroutine(Spawned());
    }
    private IEnumerator SpikeTimerOn()
    {
        yield return new WaitForSeconds(GameManager.Inst.player.invincibleTime);
        SpikeTimer = false;
    }
    protected virtual IEnumerator Spawned()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        transform.position = new(0, 5.5f);
        healthBar.localScale = new(0, 1);

        while (Mathf.Abs(transform.position.y - 2.5f) > 0.1f)
        {
            transform.position = Vector2.Lerp(transform.position, new(0, 2.5f), Time.deltaTime * 5f);
            yield return null;
        }
        transform.position = new(0, 2.5f);
        idle = StartCoroutine(Idle());
        while (healthBar.localScale.x < 49.5)
        {
            healthBar.localScale = new(Mathf.Lerp(healthBar.localScale.x, 50, Time.deltaTime * 3f), 1);
            yield return null;
        }
        healthBar.localScale = new(50, 1);
        GetComponent<BoxCollider2D>().enabled = true;
    }
    protected IEnumerator Idle()
    {
        float y;
        y = transform.position.y + 0.05f;
        while (true)
        {
            while (Mathf.Abs(transform.position.y - y) > 0.001f)
            {
                transform.position = transform.position + new Vector3(0, 0.001f);
                yield return null;
            }
            y = transform.position.y - 0.1f;
            while (Mathf.Abs(transform.position.y - y) > 0.001f)
            {
                transform.position = transform.position - new Vector3(0, 0.001f);
                yield return null;
            }
            y = transform.position.y + 0.1f;
        }
    }
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !GameManager.Inst.player.Invincible)
        {
            if (!SpikeTimer)
            {
                SpikeTimer = true;
                HP -= GameManager.Inst.player.SpikeDmg;
            }
            GameManager.Inst.player.HP -= dmg;
        }
    }
}
