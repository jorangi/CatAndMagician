using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SpawnedTearOfTheSea : MonoBehaviour
{
    private float timer = 0f;
    private void Awake()
    {
        timer = ItemManager.ConvertJToken<float>(GameManager.Inst.player.Items["TearOfTheSea"].data.value["val0"])[5];
    }
    private void Update()
    {
        timer -= Time.deltaTime;
        if(timer <= 0f)
        {
            gameObject.SetActive(false);
            Destroy(gameObject, 1f);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            List<GameObject> objs = GameObject.FindGameObjectsWithTag("Enemy").ToList();
            for (int i = objs.Count - 1; i >= 0; i--)
            {
                if (objs[i].layer != 10 && objs[i].layer != 11)
                {
                    objs.Remove(objs[i]);
                    continue;
                }
                if (!objs[i].GetComponent<Enemy>().sprite.enabled)
                {
                    objs.Remove(objs[i]);
                    continue;
                }
            }
            if (objs.Count > 0)
            {
                if (GameManager.Inst.player.itemEvos["InlaidAquamarineRing"])
                {
                    foreach(GameObject obj in objs)
                    {
                        obj.GetComponent<Enemy>().HP -= ItemManager.ConvertJToken<float>(GameManager.Inst.player.Items["TearOfTheSea"].data.value["val0"])[6];
                    }
                }
                else
                {
                    GameObject obj = objs[Random.Range(0, objs.Count)];
                    obj.GetComponent<Enemy>().HP -= ItemManager.ConvertJToken<float>(GameManager.Inst.player.Items["TearOfTheSea"].data.value["val0"])[6];
                }
            }
            float[] t = ItemManager.ConvertJToken<float>(GameManager.Inst.player.Items["TearOfTheSea"].data.value["val0"]);
            GameManager.Inst.player.GetTear(t[8], t[7]);
            gameObject.SetActive(false);
            Destroy(gameObject, 1f);
        }
    }
}
