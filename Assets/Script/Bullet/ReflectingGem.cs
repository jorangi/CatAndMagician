using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectingGem : MonoBehaviour
{
    public RevolutionGemSpawner spawner;
    public SpriteRenderer bright;
    public bool adhesion = false;
    public Quaternion euler = Quaternion.identity;
    private float Delay = 0.0f;
    private float delay = 0.0f;

    private void OnEnable()
    {
        adhesion = false;
        transform.rotation = euler;
        StartCoroutine(Move());
    }
    private IEnumerator Move()
    {
        while(!adhesion)
        {
            transform.Translate(3f * Time.deltaTime * Vector3.up);
            yield return null;
        }
    }
    private void Update()
    {
        if(adhesion)
        {
            delay = ItemManager.ConvertJToken<float>(spawner.data.value["val0"])[5];

            if(spawner.Reflecting)
            {
                Delay += Time.deltaTime;
                if (Delay >= delay)
                {
                    bright.color = new(1, 1, 1, 1f);
                    Delay = 0.0f;
                }
                else
                {
                    bright.color = new(1, 1, 1, Mathf.Lerp(bright.color.a, 0f, Delay));
                }
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Border"))
        {
            adhesion = true;
        }
    }
}
