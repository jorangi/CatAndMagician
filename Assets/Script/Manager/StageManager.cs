using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GoogleMobileAds.Api;

public class StageManager : MonoBehaviour
{
    private bool AppearedBoss = false;
    public AudioSource bgm;
    public Image backGround;
    public bool gamePlaying = true;
    private InterstitialAd interstitial;
    public Stage[] Allstages;
    public Stage stage;
    public float stageTime;
    public float currentTime;
    public List<int> waveTime;
    public List<int> namedTime;
    public List<Coroutine> nonWaveTimes = new();
    float sec;
    int min;

    private void Awake()
    {
        MobileAds.Initialize(initStatus => { RequestInterstitial(); }) ;
        RequestInterstitial();
        StageInit();
    }
    private void StageInit()
    {
        bgm = GetComponent<AudioSource>();
        backGround.sprite = stage.backGround;
        bgm.clip = stage.bgm;
        bgm.Play();
        stageTime = stage.stageTime;
        waveTime = new();
        namedTime = new();
        foreach (var w in stage.wave.Keys)
        {
            waveTime.Add(w);
        }
        foreach (var w in stage.namedEnemies.Keys)
        {
            namedTime.Add(w);
        }
        for (int i = 0; i < stage.NonWaveMulitply; i++)
        {
            nonWaveTimes.Add(StartCoroutine(NonWaveTime()));
        }
    }
    private IEnumerator NonWaveTime()
    {
        yield return new WaitForSeconds(Random.Range(stage.NonWaveDelayMin, stage.NonWaveDelayMax + 1));
        while (currentTime<=stageTime)
        {
            GameManager.Inst.enemyManager.EnemySpawn(stage.appearEnemies[Random.Range(0, stage.appearEnemies.Length)]);
            yield return new WaitForSeconds(Random.Range(stage.NonWaveDelayMin, stage.NonWaveDelayMax + 1));
        }
    }
    private IEnumerator SpawnWave()
    {
        Queue<string> wave = new();
        List<string> tempWave = new();
        foreach(KeyValuePair<string, int> mon in stage.wave[waveTime[0]].monsters)
        {
            for(int i = 0; i<mon.Value; i++)
            {
                tempWave.Add(mon.Key);
            }
        }
        for(int i = 0; i<tempWave.Count; i++)
        {
            (tempWave[i], tempWave[Random.Range(0, tempWave.Count)]) = (tempWave[Random.Range(0, tempWave.Count)], tempWave[i]);
        }
        foreach (string monster in tempWave)
        {
            wave.Enqueue(monster);
        }
        while(wave.Count > 0)
        {
            GameManager.Inst.enemyManager.EnemySpawn(wave.Peek());
            wave.Dequeue();
            yield return new WaitForSeconds(Random.Range(0f, 1f));
        }
    }
    private IEnumerator SpawnNamed()
    {
        Queue<string> wave = new();
        List<string> tempWave = new();
        foreach(KeyValuePair<string, int> mon in stage.namedEnemies[namedTime[0]].monsters)
        {
            for(int i = 0; i<mon.Value; i++)
            {
                tempWave.Add(mon.Key);
            }
        }
        for(int i = 0; i<tempWave.Count; i++)
        {
            (tempWave[i], tempWave[Random.Range(0, tempWave.Count)]) = (tempWave[Random.Range(0, tempWave.Count)], tempWave[i]);
        }
        foreach (string monster in tempWave)
        {
            wave.Enqueue(monster);
        }
        while(wave.Count > 0)
        {
            GameManager.Inst.enemyManager.EnemySpawn(wave.Peek());
            wave.Dequeue();
            yield return new WaitForSeconds(Random.Range(0f, 1f));
        }
    }
    private void Update()
    {
        if (!gamePlaying)
            return;
        if(currentTime >= stageTime)
        {
            for (int i = 0; i < nonWaveTimes.Count; i++)
            {
                StopCoroutine(nonWaveTimes[i]);
            }
            if(!AppearedBoss)
            {
                SpawnBoss();
                AppearedBoss = true;
            }
        }
        else
        {
            currentTime += Time.deltaTime;
            sec += Time.deltaTime;
            GameManager.Inst.player.stageBar.fillAmount = currentTime / stageTime;

            if (waveTime.Count>0 && waveTime[0] == (int)currentTime)
            {
                StartCoroutine(SpawnWave());
                waveTime.RemoveAt(0);
            }
            if (namedTime.Count > 0 && namedTime[0] == (int)currentTime)
            {
                StartCoroutine(SpawnNamed());
                namedTime.RemoveAt(0);
            }
        }
        GameManager.Inst.player.Timer.text = string.Format("{0:D2}:{1:D2}", min, (int)sec);
        if((int)sec > 59)
        {
            sec = 0;
            min++;
        }
    }
    public void SpawnBoss()
    {
        StartCoroutine(bgmChange());
        GameManager.Inst.enemyManager.EnemySpawn("teddywhale");
        //StageClear();
    }
    public IEnumerator bgmChange()
    {
        while(bgm.volume>0)
        {
            bgm.volume -= Time.deltaTime * 5f;
            yield return null;
        }
        bgm.clip = stage.bossBgm;
        bgm.Play();
        while(bgm.volume<1)
        {
            bgm.volume += Time.deltaTime * 5f;
            yield return null;
        }
    }
    public void StageClear()
    {
        gamePlaying = false;

        AdStart();
    }
    public void dead()
    {
        gamePlaying = false;

        AdStart();
    }
    private void AdStart()
    {
        if (!GameManager.Inst.AdBlocked)
        {
            if (interstitial.IsLoaded())
            {
                interstitial.Show();
            }
        }
    }
    private void RequestInterstitial()
    {
#if UNITY_ANDROID
        string adUnitId = "ca-app-pub-8983376303504670/6089830898";
#elif UNITY_IPHONE
            string adUnitId = "ca-app-pub-3940256099942544/4411468910";
#else
            string adUnitId = "unexpected_platform";
#endif

        // Initialize an InterstitialAd.
        this.interstitial = new InterstitialAd(adUnitId);
        AdRequest request = new AdRequest.Builder().Build();
        this.interstitial.LoadAd(request);
    }
}
