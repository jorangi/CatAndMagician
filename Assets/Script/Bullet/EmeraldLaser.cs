using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmeraldLaser : InlaidProjectile
{
    public float timer = 0f;
    private void Update()
    {
        timer -= Time.deltaTime;
        if(timer <= 0f)
        {
            gameObject.SetActive(false);
        }
    }
}
