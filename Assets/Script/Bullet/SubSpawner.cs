using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubSpawner : MonoBehaviour
{
    public GameObject MainSpawner;
    private void Update()
    {
        transform.RotateAround(transform.parent.position, Vector3.back, Mathf.CeilToInt(Time.deltaTime) * 5f);
    }
}
