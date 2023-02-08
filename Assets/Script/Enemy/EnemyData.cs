using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Data", menuName = "Scriptable Object/Enemy Data", order = int.MaxValue)]
public class EnemyData : ScriptableObject
{
    //0 : ����ġ ����, 1 : �����, 2 : �Ķ���, 3 : �ʷϺ�, 4 : ������, 5 : �ϾẰ
    public int expLv;
    public float dmg;
    public float spd;
    public float knockbackResist;
    public float ccResist;
    public int hp;
    public int shield;
}
