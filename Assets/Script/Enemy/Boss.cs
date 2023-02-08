using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    protected Coroutine idle;
    protected override void Awake()
    {
        base.Awake();
        healthBar.SetParent(null);
        healthBar.position = new(-2.5f, 4.5f);
        shieldBar.SetParent(null);
        shieldBar.position = new(-2.5f, 4.5f);

        StartCoroutine(Spawned());
    }
    protected virtual IEnumerator Spawned()
    {
        triggerCol.enabled = false;
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
        triggerCol.enabled = true;
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
    protected override void RefreshHPBar()
    {
        base.RefreshHPBar();
        if (Shield == 0)
        {
            healthBar.localScale = new(50 * (HP / maxhp), 1);
            shieldBar.localScale = new(0, 1);
        }
        else
        {
            healthBar.localScale = new(HP * 50 / Mathf.Max(HP + Shield, maxhp), 1);
            shieldBar.localScale = new(Shield * 50 / Mathf.Max(HP + Shield, maxhp), 1);
            shieldBar.localPosition = new(-0.5f + (healthBar.localScale.x / 10f), -0.65f);
        }
    }
}
