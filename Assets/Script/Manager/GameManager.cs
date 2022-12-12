using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public string lang = "ko";
    public bool AdBlocked = false;
    private static GameManager inst = null;
    public EnemyManager enemyManager;
    public ConsoleManager consoleManager;
    public StageManager stageManager;
    public Player player;
    public StrGameObjDictionary Drops = new();
    public static GameManager Inst
    {
        get
        {
            if(!inst)
            {
                inst = FindObjectOfType<GameManager>();
            }
            return inst;
        }
    }
    private void Awake()
    {
        if(inst == null)
        {
            inst = this;
        }
        else if(inst != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        Application.targetFrameRate = 120;
        enemyManager = FindObjectOfType<EnemyManager>();
        consoleManager = FindObjectOfType<ConsoleManager>();
        stageManager = FindObjectOfType<StageManager>();
        player = FindObjectOfType<Player>();
    }
}
