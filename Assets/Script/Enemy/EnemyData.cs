using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Data", menuName = "Scriptable Object/Enemy Data", order = int.MaxValue)]
public class EnemyData : ScriptableObject
{
    //0 : 경험치 없음, 1 : 노란별, 2 : 파란별, 3 : 초록별, 4 : 붉은별, 5 : 하얀별
    public int expLv;
    public float dmg;
    public float spd;
    public float knockbackResist;
    public float ccResist;
    public int hp;
    public int shield;
}
