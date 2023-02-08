using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public void Remove()
    {
        gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
