using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Stage", menuName = "Scriptable Object/Stage", order = int.MaxValue)]
public class Stage : ScriptableObject
{
    public AudioClip bgm;
    public AudioClip bossBgm;
    public float stageTime;
    public int NonWaveMulitply;
    public float NonWaveDelayMin;
    public float NonWaveDelayMax;
    public string stageId;
    public string stageName;
    public Sprite backGround;
    public string[] appearEnemies;
    public IntWaveDictionary namedEnemies = new();
    public IntWaveDictionary wave = new();
    public string bossEnemy;
}