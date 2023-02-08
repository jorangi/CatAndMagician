using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LunaApple : Item
{
    private float timer;
    private void Update()
    {
        if(Evo)
        {
            timer -= Time.deltaTime;
            if(timer <= 0)
            {
                timer = ItemManager.ConvertJToken<float>(data.value["val1"])[0];
                Debug.Log("»ç°ú ÅõÃ´");
            }
        }
    }
}
