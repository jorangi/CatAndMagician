using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnedJewelryFlower : Bullet
{
    private float timer = 0f;
    protected override void Awake()
    {
        base.Awake();
        entity = GameManager.Inst.player.Items["JewelryFlower"];
        timer = ItemManager.ConvertJToken<float>(GameManager.Inst.player.Items["JewelryFlower"].data.value["val0"])[6];
    }
    private void Update()
    {
        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            gameObject.SetActive(false);
            Destroy(gameObject, 1f);
        }
    }
    protected override void FixedUpdate()
    {
    }
    protected override void HitEnemy(Enemy enemy)
    {
        float[] _t = ItemManager.ConvertJToken<float>(GameManager.Inst.player.Items["JewelryFlower"].data.value["val0"]);
        enemy.HP += enemy.ChangeHS(true, -_t[GameManager.Inst.player.itemLevels["JewelryFlower"] - 1] * GameManager.Inst.player.BulletDmgRatio + GameManager.Inst.player.NumericalBulletDmg);
        enemy.GetCC("JewerlyFlower", "slow", _t[8] * 0.01f, _t[7]);
        if (GameManager.Inst.player.itemEvos["JewelryFlower"])
        {
            float[] t = ItemManager.ConvertJToken<float>(GameManager.Inst.player.Items["JewelryFlower"].data.value["val1"]);
            enemy.GetCC("JewerlyFlower", "poison", t[0], t[1]);
        }
        ReturnObject();
    }
}
