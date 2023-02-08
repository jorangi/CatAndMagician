using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmethystRange : MonoBehaviour
{
    private float timer = 0.1f;
    private void Update()
    {
        if(timer <= 0)
        {
            gameObject.SetActive(false);
        }
        timer -= Time.deltaTime;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy"))
        {
            collision.transform.parent.GetComponent<Enemy>().GetCC("InlaidAmethystRing", "slow", ItemManager.ConvertJToken<float>(GameManager.Inst.player.Items["InlaidAmethystRing"].data.value["val1"])[1], ItemManager.ConvertJToken<float>(GameManager.Inst.player.Items["InlaidAmethystRing"].data.value["val1"])[0]);
        }
    }
}
