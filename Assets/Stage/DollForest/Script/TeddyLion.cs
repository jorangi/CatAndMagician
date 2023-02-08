using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeddyLion : NormalEnemy
{
    private bool LaserShape = false;
    public GameObject Laser;
    protected override void Awake()
    {
        base.Awake();
        LaserOn();
    }
    private void Start()
    {
        transform.position = new(0, 5.5f);
    }
    public void LaserOn()
    {
        switch(LaserShape)
        {
            case false:
                XLaser();
                LaserShape = true;
                break;
            case true:
                PlusLaser();
                LaserShape = false;
                break;
        }
    }
    public void XLaser()
    {
        GameObject l1 = Instantiate(Laser, transform);
        GameObject l2 = Instantiate(Laser, transform);
        l1.GetComponent<LionLaser>().parent = this;
        l1.GetComponent<LionLaser>().once = true;
        l2.GetComponent<LionLaser>().parent = this;
        l1.transform.rotation = Quaternion.Euler(0, 0, -45);
        l2.transform.rotation = Quaternion.Euler(0, 0, 45);
    }
    public void PlusLaser()
    {

        GameObject l1 = Instantiate(Laser, transform);
        GameObject l2 = Instantiate(Laser, transform);
        l1.GetComponent<LionLaser>().parent = this;
        l1.GetComponent<LionLaser>().once = true;
        l2.GetComponent<LionLaser>().parent = this;
        l1.transform.rotation = Quaternion.Euler(0, 0, 90);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (transform.position.y < -5.5f && collision.CompareTag("Remove"))
        {
            Destroy(gameObject);
        }
    }
}
