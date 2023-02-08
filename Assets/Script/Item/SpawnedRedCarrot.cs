using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnedRedCarrot : MonoBehaviour
{
    private float lifeTime;
    private bool chk;
    private float dmg;
    private float timer;
    private void Awake()
    {
        dmg = FindObjectOfType<BrokenWatchSpawner>().dmg * GameManager.Inst.player.BulletDmgRatio * 0.2f;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy") && chk)
        {
            collision.transform.parent.GetComponent<Enemy>().HP -= dmg;
        }
    }
    private void Update()
    {
        lifeTime += Time.deltaTime;
        if (lifeTime >= 6)
        {
            Destroy(gameObject);
        }

        chk = false;
        if(timer <= 0)
        {
            GameObject[] objs = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject obj in objs)
            {
                obj.transform.parent.GetComponent<Enemy>().GetCC("redcarrot", "grabbed", (Vector2)transform.position, 1f);
            }
            chk = true;
            timer = 1f;
        }
        timer -= Time.deltaTime;
    }
}
