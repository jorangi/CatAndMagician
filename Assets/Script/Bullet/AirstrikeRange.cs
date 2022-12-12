using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirstrikeRange : MonoBehaviour
{
    public AirstrikeSpawner spawner;
    public float dmg;
    public float speed;
    public float knockback;
    public float timer;

    private void OnEnable()
    {
        timer = 10 / speed;
        StartCoroutine(ExpandingRange());
    }
    private IEnumerator ExpandingRange()
    {
        Transform red = transform.GetChild(0);
        red.localScale = new(0, 0, 1);
        float size = 0;
        while(timer > 0)
        {
            size += Time.deltaTime * (speed / 10);
            red.localScale = new(size, size, 1);
            timer -= Time.deltaTime;
            yield return null;
        }
        DeployMagicCircle();
    }
    private void DeployMagicCircle()
    {
        GameObject obj = spawner.Bullets.transform.Find(spawner.bulletData.name).GetChild(0).gameObject;
        obj.transform.SetParent(null);
        Bullet bullet = obj.GetComponent<Bullet>();
        bullet.dmg = dmg;
        bullet.knockback = knockback;
        obj.transform.localScale = transform.localScale;
        obj.name = spawner.bulletData.name;
        obj.transform.position = transform.position;
        obj.SetActive(true);
        StartCoroutine(Hide());
    }
    private IEnumerator Hide()
    {
        float t = 0;
        float alpha = 0;
        var a = GetComponent <SpriteRenderer>();
        var b = transform.GetChild(0).GetComponent <SpriteRenderer>();
        while(t < 0.3f)
        {
            alpha += Time.deltaTime * (1/0.3f);
            a.color = new(1, 1, 1, 1 - alpha);
            b.color = new(1, 1, 1, 1 - alpha);
            t += Time.deltaTime;
            yield return null;
        }
        transform.SetParent(GameObject.FindGameObjectWithTag("Bullets").transform.Find("AirstrikeRange"));
        gameObject.SetActive(false);
        a.color = new(1, 1, 1, 1);
        b.color = new(1, 1, 1, 1);
    }
}
