using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpStar : MonoBehaviour
{
    protected int ExpValue = 0;
    private bool Magnet = false;
    private readonly float spd = 2.0f;
    private void FixedUpdate()
    {
        if (!Magnet)
        {
            transform.Translate(spd * Time.fixedDeltaTime * Vector2.down);
        }
        else
        {
            transform.position = Vector2.Lerp(transform.position, GameManager.Inst.player.transform.position, spd * 5f * Time.fixedDeltaTime);
        }
    }
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<AudioSource>().Play();
            GetComponent<SpriteRenderer>().enabled = false;
            GameManager.Inst.player.Exp += Mathf.RoundToInt(ExpValue * (1 + ItemManager.ConvertJToken<float>(ItemManager.datas["JewelryCuckoosEgg"].value["val0"])[0] * 0.01f * GameManager.Inst.player.itemLevels["JewelryCuckoosEgg"]));
            Destroy(gameObject, 1f);
        }
        if (collision.CompareTag("Magnet"))
        {
            Magnet = true;
        }
        if (collision.CompareTag("Remove"))
        {
            Destroy(gameObject);
        }
    }
}
