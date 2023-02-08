using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tarcat : Boss
{
    float MaxDmg = 0.0f;
    public override float HP
    {
        get => hp;
        set
        {
            float t = 0;
            if(hp > value)
            {   
                t = Mathf.Max((hp - value) * Vulnerable, 0.0f); //°¨¼Ò°ª
                dps += t;
            }
            if (dps > MaxDmg)
            {
                MaxDmg = dps;
                maxDPS.text = $"{MaxDmg}";
            }
            DPS.text = $"{dps}";
            value = Mathf.Clamp(hp - t, 1, maxhp);
            hp = value;
        }
    }
    public TMPro.TextMeshPro DPS;
    public TMPro.TextMeshPro maxDPS;
    private float dps = 0f;
    private readonly WaitForSeconds onesec = new(1f);

    protected override void Awake()
    {
        maxhp = data.hp;
        MoveSpeed = 0f;
        knockbackResist = data.knockbackResist;
        Resist = data.ccResist;
        HP = maxhp;
        healthBar = null;
        shieldBar = null;
        MaxDmg = 0.0f;
        maxDPS.text = 0.ToString();
        StartCoroutine(OnesecRepeat());
    }
    private void Start()
    {
        transform.position = new(0, 2f);
    }
    protected override IEnumerator Spawned()
    {
        yield break;
    }
    protected override void FixedUpdate()
    {
        grab = false;
    }
    private IEnumerator OnesecRepeat()
    {
        while (true)
        {
            HP = data.hp;
            yield return onesec;
            DPS.text = $"{0.0f}";
            dps = 0.0f;
        }
    }
}
