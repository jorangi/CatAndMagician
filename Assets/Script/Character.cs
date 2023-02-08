using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    //컴포넌트
    public Rigidbody2D rigid;
    public Transform MicsEffect;
    public SpriteRenderer sprite;
    protected Collider2D col;
    protected Transform healthBar;
    protected Transform shieldBar;

    //이동속도 관련
    public float MoveSpeedRatio;
    private float moveSpeed;
    public float MoveSpeed
    {
        get => moveSpeed;
        set
        {
            value = Mathf.Max(0.01f, value);
            moveSpeed = value;
        }
    }

    //체력 관련
    protected float hp;
    public float maxhp;

    //상태이상 관련
    public bool stopped;
    public bool grab;
    public Vector2 grabbed;
    private float vulnerable = 1f;
    public float Vulnerable
    {
        get => vulnerable;
        set
        {
            vulnerable = value;
        }
    }
    private float resist;
    public float Resist
    {
        get => resist;
        set
        {
            resist = value;
        }
    }
    private int ccGuard;
    public int CCGuard
    {
        get => ccGuard;
        set
        {
            ccGuard = value;
        }
    }
    public List<CC> CC = new();
    public Dictionary<string, float> CCAccum = new();

    protected virtual void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        MicsEffect = transform.Find("MicsEffect");
        healthBar = transform.Find("HealthBar");
        shieldBar = transform.Find("ShieldBar");
    }
    public void GetCC(string caster, string type, object value, float time)
    {
        if (!gameObject.activeSelf)
            return;
        if (CCGuard > 0)
        {
            CCGuard--;
            return;
        }

        if (!CCAccum.ContainsKey(type))
        {
            CCAccum.Add(type, 0);
        }

        for (int i = CC.Count - 1; i >= 0; i--)
        {
            CC c = CC[i];
            if (c.caster == caster && c.type == type && c.time <= time && c.value.Equals(value))
            {
                c.time = time;
                return;
            }
        }
        CC cC;
        if(GetComponent<Player>() != null)
        {
            cC = new(caster, type, value, time * (1 - Resist), null);
        }
        else
        {
            cC = new(caster, type, value, time * (1 - Resist), (Enemy)this);
        }
        CC.Add(cC);
        StartCoroutine(cC.ApplyCC());
    }
    public void SetDot(CC cC)
    {
        switch (cC.type)
        {
            case "poison":
                cC.co = StartCoroutine(Poison(cC));
                break;
            case "bleed":
                cC.co = StartCoroutine(Bleed(cC));
                break;
        }
    }
    public void StopDot(CC cC)
    {
        StopCoroutine(cC.co);
    }
    protected virtual IEnumerator Poison(CC cc)
    {
        yield break;
    }
    protected virtual IEnumerator Bleed(CC cc)
    {
        yield break;
    }
    protected virtual void RefreshHPBar()
    {

    }
}
