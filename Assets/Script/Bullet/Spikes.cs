using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    private void FixedUpdate()
    {
        if (transform.childCount == 0)
            Destroy(gameObject);
    }
}
