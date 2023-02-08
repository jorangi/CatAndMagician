using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicKnifeSpawner : BulletSpawner
{
    private MagicKnife knife;
    public int spawnCount = 0;
    public bool returned = true;
    protected override void PoolingBullet()
    {
        base.PoolingBullet();
        knife = Bullets.transform.Find(data.value["id"].ToString()).GetChild(0).GetComponent<MagicKnife>();
        knife.name = "MagicKnife";
    }
    protected override void Update()
    {
        if (knife == null || Lv == 0 || name != "BulletSpawner")
            return;
        if (!knife.gameObject.activeSelf)
        {
            returned = true;
        }
        if (returned)
            ShootBullet();
    }
    protected override void ShootBullet()
    {
        GameObject enemy = GameObject.FindGameObjectWithTag("Enemy");
        if (enemy == null)
        {
            return;
        }
        if (enemy.layer == 14)
        {
            enemy = enemy.transform.parent.gameObject;
        }
        if (!enemy.GetComponent<Enemy>().inScreen || !enemy.GetComponent<SpriteRenderer>().enabled)
        {
            return;
        }

        spawnCount++;
        returned = false;
        knife.transform.SetParent(null);
        knife.e.Clear();
        knife.Bullets = Bullets.transform.Find(data.value["id"].ToString());
        if(GameManager.Inst.player.itemLevels["OrnamentMagicKnife"] > 0)
        {
            List<GameObject> t = GameObject.FindGameObjectsWithTag("Enemy").ToList();
            for(int i = t.Count-1; i >= 0; i--)
            {
                if (t[i].layer != 10 && t[i].layer != 11)
                {
                    t.Remove(t[i]);
                    continue;
                }
                if (!t[i].GetComponent<Enemy>().inScreen)
                {
                    t.Remove(t[i]);
                    continue;
                }
            }
            for (int i = 0; i < t.Count; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    if (Vector2.Distance(GameManager.Inst.player.transform.position, t[i].transform.position) < Vector2.Distance(GameManager.Inst.player.transform.position, t[j].transform.position))
                    {
                        (t[i], t[j]) = (t[j], t[i]);
                    }
                }
            }
            if (spawnCount == 7)
            {
                knife.e = t;
                spawnCount = 0;
            }
            else
            {
                knife.e.Add(t[Mathf.Max(0, t.Count - 1)]);
            }
        }
        else
        {
            List<GameObject> t = GameObject.FindGameObjectsWithTag("Enemy").ToList();
            for (int i = 0; i < t.Count; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    if (Vector2.Distance(GameManager.Inst.player.transform.position, t[i].transform.position) < Vector2.Distance(GameManager.Inst.player.transform.position, t[j].transform.position))
                    {
                        (t[i], t[j]) = (t[j], t[i]);
                    }
                }
            }
            knife.e.Add(t[0]);
        }
        Bullet bullet = knife.GetComponent<Bullet>();
        bullet.MaxPierce = GameManager.Inst.player.PiercingTarget;
        knife.transform.localScale = new(Size, Size);
        knife.name = data.value["id"].ToString();
        knife.transform.position = transform.position;
        GameManager.Inst.player.recentProjectile = bullet;
        GameManager.Inst.player.Projectile++;
        knife.gameObject.SetActive(true);
    }
}
