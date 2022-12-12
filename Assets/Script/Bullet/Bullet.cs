using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    public float dmg;
    public float knockback;
    protected virtual void Awake()
    {
        GetComponent<BoxCollider2D>().isTrigger = true;
    }
    protected virtual void Update()
    {
        transform.Translate(speed * Time.deltaTime * Vector2.up);
    }
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
    }
    public void ReturnObject()
    {
        transform.SetParent(GameObject.FindGameObjectWithTag("Bullets").transform.Find(name));
        gameObject.SetActive(false);
    }
}
