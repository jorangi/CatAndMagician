using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackFeather : Bullet
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.CompareTag("Enemy"))
        {
            Transform MicsEffect = collision.transform.parent.Find("MicsEffect");
            if (entity.Evo)
            {
                if (!MicsEffect.Find("BlackWingMark").gameObject.activeSelf)
                {
                    MicsEffect.Find("BlackWingMark").gameObject.SetActive(true);
                    MicsEffect.Find("BlackWingMark").GetComponent<BlackWingMark>().Count++;
                }
                else
                {
                    MicsEffect.Find("BlackWingMark").GetComponent<BlackWingMark>().Count++;
                }
            }
        }
    }
}
