using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Bullet Data", menuName = "Scriptable Object/Bullet Data", order = int.MaxValue)]
public class BulletData : ScriptableObject
{
    public GameObject bullet;
    public float[] delay;
    public float[] speed;
    public float[] dmg;
    public float knockback;
    public int limit;
}
