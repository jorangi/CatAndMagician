using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirstrikeSpawner : BulletSpawner
{
    public GameObject AirstrikeRange;
    public Sprite AirstrikeRangeRed;
    private bool TwiceD;
    private int FireCount;
    private float Timer;

    protected override void Update()
    {
        base.Update();
        if(Timer>0)
        {
            Timer -= Time.deltaTime;
        }
        if(TwiceD && Timer <= 0)
        {
            delay /= 2f;
            TwiceD = false;
        }
    }
    protected override void PoolingBullet()
    {
        base.PoolingBullet();
        GameObject parent = new();
        parent.transform.SetParent(Bullets.transform);
        parent.name = "AirstrikeRange";

        for (int i = 0; i < (int)data.value["limit"]; i++)
        {
            GameObject obj = Instantiate(AirstrikeRange, parent.transform);
            obj.GetComponent<Bullet>().entity = this;
            obj.name = "AirstrikeRange";
            obj.SetActive(false);
        }

    }
    protected override void ShootBullet()
    {
        FireCount++;
        if(FireCount%10==0)
        {
            delay *= 2;
            Timer = 2f;
            TwiceD = true;
        }
        GameObject obj = Bullets.transform.Find("AirstrikeRange").GetChild(0).gameObject;
        obj.transform.SetParent(null);
        obj.transform.localScale = new(GameManager.Inst.player.BulletSize, GameManager.Inst.player.BulletSize);
        obj.name = "AirstrikeRange";
        if(!GameObject.FindGameObjectWithTag("Enemy"))
        {
            obj.transform.position = new(transform.position.x, 3);
        }
        else
        {
            bool chk = true;
            Enemy[] e = FindObjectsOfType<Enemy>();

            foreach (Enemy enemy in e)
            {
                if (enemy.inScreen)
                {
                    chk = false;
                    break;
                }
            }
            if(chk)
            {
                obj.transform.position = new(transform.position.x, 3);
                obj.SetActive(true);
                return;
            }
            obj.transform.position = e[0].transform.position;
        }
        obj.SetActive(true);
    }
}
