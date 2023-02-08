using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SapphireWall : MonoBehaviour
{
    private float timer = 0f;

    private void Awake()
    {
        timer = ItemManager.ConvertJToken<float>(GameManager.Inst.player.Items["InlaidSapphireRing"].data.value["val0"])[1];
    }
    private void Update()
    {
        timer-= Time.deltaTime;
        if(timer <= 0)
        {
            gameObject.SetActive(false);
            Destroy(gameObject, 1f);
        }
    }
}