using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDrop : MonoBehaviour
{
    public Enemy parent;
    public Rigidbody2D rigid;
    private float oriX;
    private float desX;
    private float posY;
    private float acc;
    private void Awake()
    {
        posY = transform.position.y;
        float scale = Random.Range(0.7f, 1f);
        transform.localScale = new(scale, scale);
        transform.rotation = Quaternion.Euler(0, 0, 180f);
        rigid.AddForce(Vector2.up * Random.Range(9f, 10f), ForceMode2D.Impulse);
        desX = Random.Range(-2f, 2f);
        oriX = transform.position.x;
        acc = Random.Range(0.7f, 1.3f);
    }
    private void FixedUpdate()
    {
        if(posY > transform.position.y) //ÇÏ°­
        {
            transform.position = new(transform.position.x + desX * acc * Time.deltaTime, transform.position.y);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, 180 - Mathf.Sign(desX) * 180f), Time.deltaTime * 5f);
        }
        else //»ó½Â
        {
            transform.Rotate(0, 0, -Mathf.Sign(desX) * Time.deltaTime * 100f * Mathf.Abs((desX - oriX)/(oriX + Mathf.Sign(oriX) * 2)));
            transform.position = new(transform.position.x + desX * acc * Time.deltaTime * 1.3f, transform.position.y);
        }
        posY = transform.position.y;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Remove"))
        {
            Destroy(gameObject);
        }
        else if(collision.CompareTag("Player"))
        {
            GameManager.Inst.player.Hit(parent, 8);
        }
    }
}
