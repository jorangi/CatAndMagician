using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicStone : Item
{
    public Sprite subSpawner;
    protected override void Evolved()
    {
        StartCoroutine(SubSpawner());
    }
    protected override void LevelChanged()
    {
        base.LevelChanged();
        float[] val = ItemManager.ConvertJToken<float>(data.value["val0"]);
        GameManager.Inst.player.DelayRatio *= 1 + val[Mathf.Min(val.Length - 1, Lv - 1)] * 0.01f;
    }
    private IEnumerator SubSpawner()
    {
        while(true)
        {
            yield return new WaitForSeconds(ItemManager.ConvertJToken<float>(data.value["val1"])[0]);
            GameObject obj = Instantiate(GameManager.Inst.player.transform.Find("BulletSpawner").gameObject, GameManager.Inst.player.transform);
            obj.AddComponent<SpriteRenderer>().sprite = subSpawner;
            float d = ItemManager.ConvertJToken<float>(data.value["val1"])[1];
            while(d > 0)
            {
                d -= Time.deltaTime;
                obj.transform.RotateAround(transform.parent.position, Vector3.back, Mathf.CeilToInt(Time.deltaTime) * 5f * GameManager.Inst.player.BulletSpeedRatio);
                yield return null;
            }
            Destroy(obj);
        }
    }
}
