using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnedBlackCarrot : MonoBehaviour
{
    private float lifeTime;
    private GameObject player;
    private void Awake()
    {
        player = GameManager.Inst.player.gameObject;
    }
    private void Update()
    {
        lifeTime += Time.deltaTime;
        if (lifeTime >= ItemManager.ConvertJToken<float>(ItemManager.datas["carrot"].value["val0"])[6])
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            player.layer = 12;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player.layer = 8;
        }
    }
}
