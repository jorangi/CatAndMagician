using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NearbyPlayer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy"))
        {
            collision.transform.parent.GetComponent<Enemy>().NearbyPlayer = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.transform.parent.GetComponent<Enemy>().NearbyPlayer = false;
        }
    }
}
