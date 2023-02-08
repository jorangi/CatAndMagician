using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Airstrike : WideBullet
{
    protected override void OnEnable()
    {
        base.OnEnable();
        StartCoroutine(Hide());
    }
    private IEnumerator Hide()
    {
        yield return new WaitForSeconds(0.05f);
        col.enabled = false;
        float t = 0;
        while (t < 0.95f)
        {
            t += Time.deltaTime;
            sprite.color = new(1, 1, 1, 1-t);
            yield return null;
        }
        ReturnObject();
        sprite.color = new(1, 1, 1, 1);
    }
    protected override void FixedUpdate()
    {
    }
}
