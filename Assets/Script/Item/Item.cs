using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemData data;
    public GameObject itemSlot;
    private bool Magnet = false;
    private float spd = 2.0f;
    public virtual void SetLv(int lv)
    {
    }
    public virtual void AddLv()
    {
    }
    public virtual void SubLv()
    {
    }
    private void FixedUpdate()
    {
        if( !Magnet )
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
        if(collision.CompareTag("Player"))
        {
            GetComponent<BoxCollider2D>().enabled = false;
            Destroy(gameObject);
        }
        if(collision.CompareTag("Magnet"))
        {
            Magnet = true;
        }
        if (collision.CompareTag("Remove"))
        {
            Destroy(gameObject);
        }
    }
}
