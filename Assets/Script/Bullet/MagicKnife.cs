using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MagicKnife : Bullet
{
    private float oriDmg;
    private GameObject recentTarget;
    private int recentTargetCount = 0;
    public List<GameObject> e = new();
    protected override void Awake()
    {
        base.Awake();
        entity = GameManager.Inst.player.Items["MagicKnife"] as MagicKnifeSpawner;
    }
    protected override void FixedUpdate()
    {
        if (e.Count > 0)
        {
            Vector3 tPos;
            if (e[0] == null || !e[0].GetComponent<SpriteRenderer>().enabled)
            {
                e.RemoveAt(0);
                tPos = GameManager.Inst.player.transform.position;
            }
            else
            {
                tPos = e[0].transform.position;
            }
            Vector3 Pos = transform.position;
            transform.SetPositionAndRotation(Vector2.MoveTowards(transform.position, tPos, Time.fixedDeltaTime * Speed), Quaternion.Euler(0, 0, Mathf.Atan2(tPos.y - Pos.y, tPos.x - Pos.x) * Mathf.Rad2Deg - 90));
        }
        else
        {
            Vector3 tPos = GameManager.Inst.player.transform.position;
            Vector3 Pos = transform.position;
            transform.SetPositionAndRotation(Vector2.MoveTowards(transform.position, GameManager.Inst.player.transform.position, Time.fixedDeltaTime * Speed), Quaternion.Euler(0, 0, Mathf.Atan2(tPos.y - Pos.y, tPos.x - Pos.x) * Mathf.Rad2Deg - 90));
        }
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            oriDmg = dmg;
            if(recentTarget == collision.gameObject)
            {
                recentTargetCount++;
                if(entity.Evo)
                {
                    if(Mathf.Pow(1 - (ItemManager.ConvertJToken<float>(ItemManager.datas["MagicKnife"].value["val1"])[0] * 0.01f), recentTargetCount) <= ItemManager.ConvertJToken<float>(ItemManager.datas["MagicKnife"].value["val1"])[1] * 0.01f)
                    dmg *= 1 - (ItemManager.ConvertJToken<float>(ItemManager.datas["MagicKnife"].value["val1"])[0] * 0.01f);
                }
                else
                {
                    dmg *= 1 - (ItemManager.ConvertJToken<float>(ItemManager.datas["MagicKnife"].value["val0"])[0] * 0.01f);
                }
            }
            else
            {
                dmg = oriDmg;
                recentTargetCount = 0;
            }
            base.OnTriggerEnter2D(collision);
            recentTarget = collision.gameObject;
            if(e.Count>0 && collision.gameObject != null && e[0] == collision.transform.parent.gameObject)
            {
                if (GameManager.Inst.player.itemLevels["OrnamentMagicKnife"] > 0 && (entity as MagicKnifeSpawner).spawnCount == ItemManager.ConvertJToken<int>(ItemManager.datas["OrnamentMagicKnife"].value["val1"])[0])
                {
                    e.Remove(collision.gameObject);
                }
                else
                {
                    e.Clear();
                }
            }
            if (GameManager.Inst.player.OrnamentMagicKnife)
            {
                dmg *= 1 + (ItemManager.ConvertJToken<float>(ItemManager.datas["OrnamentMagicKnife"].value["val0"])[Mathf.Min(ItemManager.ConvertJToken<float>(ItemManager.datas["OrnamentMagicKnife"].value["val0"]).Length - 1, GameManager.Inst.player.itemLevels["OrnamentMagicKnife"] - 1)] * 0.01f);
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && e.Count == 0)
        {
            ReturnObject();
            (entity as MagicKnifeSpawner).returned = true;
        }
    }
    protected override IEnumerator ReturnObj()
    {
        gameObject.SetActive(false);
        yield break;
    }
}
