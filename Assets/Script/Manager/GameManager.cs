using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<bool> isStopped = new();
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
    public ItemManager itemManager;
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
        InitStopped();
    }
    private void Update()
    {
        bool result = false;

        foreach (bool stop in isStopped)
        {
            if (stop)
            {
                result = true;
                break;
            }
        }
        if (result)
            Time.timeScale = 0.0f;
        else
            Time.timeScale = 1.0f;
    }
    private void InitStopped()
    {
        isStopped.Add(false); // Pause메뉴용
        isStopped.Add(false); // 콘솔메뉴용
        isStopped.Add(false); // 레벨업메뉴용
        isStopped.Add(false); // 진화메뉴용
    }
}
