using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnedCarrot : MonoBehaviour
{
    private float lifeTime;
    private float val;
    private CircleCollider2D col;
    private void Awake()
    {
        val = ItemManager.ConvertJToken<float>(ItemManager.datas["carrot"].value["val0"])[Mathf.Min(ItemManager.ConvertJToken<float>(ItemManager.datas["carrot"].value["val0"]).Length - 1, GameManager.Inst.player.itemLevels["carrot"] - 1)];
    }
    private void Update()
    {
        lifeTime += Time.deltaTime;
        if(lifeTime >= ItemManager.ConvertJToken<float>(ItemManager.datas["carrot"].value["val0"])[6])
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy"))
        {
            collision.transform.parent.GetComponent<Enemy>().GetCC("carrot", "slow", val, 100f);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy"))
        {
            Enemy e = collision.transform.parent.GetComponent<Enemy>();
            foreach (CC cc in e.CC)
            {
                if(cc.caster == "carrot" && cc.type == "slow" && (float)cc.value == val)
                {
                    cc.Remove();
                }
            }
        }
    }
}
