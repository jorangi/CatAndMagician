using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carrot : Item
{
    public GameObject carrot, redCarrot, blackCarrot;
    private float timer;
    private void Update()
    {
        timer += Time.deltaTime;
        if(timer >= ItemManager.ConvertJToken<float>(data.value["val0"])[5])
        {
            GameObject obj = Instantiate(carrot);
            obj.transform.position = Vector2.zero;
            timer = 0f;
        }
    }
    public void SpawnBlackCarrot()
    {
        GameObject obj = Instantiate(blackCarrot);
        obj.transform.position = Vector2.zero;
    }
    public void SpawnRedCarrot()
    {
        GameObject obj = Instantiate(redCarrot);
        obj.transform.position = Vector2.zero;
    }
}
