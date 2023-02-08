using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevolutionGemSpawner : BulletSpawner
{
    private WaitForSeconds d;
    public bool Reflecting;
    public GameObject ReflectingGem;
    public GameObject Laser;
    public bool Hitted = false;
    private int count = 0;
    public int RCount = 0;
    public int Count
    {
        get => count;
        set
        {
            if(count != value)
            {
                CreateGem(value);
            }
            count = value;
        }
    }
    public List<RevolutionGem> gems;
    List<GemLaser> lasers = new();
    private ReflectingGem[] rGems;
    private Coroutine reflect;
    private Coroutine revolutioning;
    private float revSpeed = 1f;
    public float RevSpeed
    {
        get => revSpeed;
        set
        {
            value = Mathf.Clamp(value, 1, 1 + (ItemManager.ConvertJToken<float>(data.value["val0"])[1] * 0.01f));
            revSpeed = value;
        }
    }
    protected override void Awake()
    {
        base.Awake();
        d = new(ItemManager.ConvertJToken<float>(data.value["val0"])[2]);
    }
    protected override void Update()
    {
        if (Lv == 0 || name != "BulletSpawner")
            return;

        Count = (int)(delay * GameManager.Inst.player.DelayRatio);

        if(gems.Count > 0)
        {
            if (RevSpeed <= 1 + (ItemManager.ConvertJToken<float>(data.value["val0"])[4] * 0.01f) && Evo && reflect != null)
            {
                StartCoroutine(GatherReflectingGem());
            }
            if (RevSpeed.Equals(1 + (ItemManager.ConvertJToken<float>(data.value["val0"])[1] * 0.01f)) && Evo && reflect == null)
            {
                FireReflectingGem();
            }
        }
    }
    protected override void PoolingBullet()
    {
        base.PoolingBullet();

        GameObject parent = new();
        parent.transform.SetParent(Bullets.transform);
        parent.name = "ReflectingGem";

        for (int i = 0; i < (int)data.value["limit"]; i++)
        {
            GameObject obj = Instantiate(ReflectingGem, parent.transform);
            obj.name = "ReflectingGem";
            obj.SetActive(false);
        }

        GameObject lparent = new();
        lparent.transform.SetParent(Bullets.transform);
        lparent.name = "GemLaser";
        for (int i = 0; i < (int)data.value["limit"] * ((int)data.value["limit"] - 1)/2; i++)
        {
            GameObject obj = Instantiate(Laser, lparent.transform);
            obj.GetComponent<Bullet>().entity = this;
            obj.name = "GemLaser";
            obj.SetActive(false);
        }
    }
    protected override void ShootBullet()
    {
    }
    protected override void ChangeSize()
    {
        CreateGem(Count);
    }
    private void CreateGem(int c)
    {
        //모든 보석 제거
        foreach(RevolutionGem gem in gems)
        {
            gem.HitTarget.Clear();
            gem.transform.SetParent(gem.Bullets);
            gem.gameObject.SetActive(false);
        }
        //보석 생성
        for (int i = 0; i < c; i++)
        {
            Transform gem = Bullets.transform.Find(data.value["id"].ToString()).GetChild(0);
            gem.SetParent(GameManager.Inst.player.transform);
            gem.gameObject.SetActive(true);
            gem.localPosition = Quaternion.Euler(0, 0, 360 / c * i) * new Vector3(Size, 0, 0);
            Bullet bullet = gem.GetComponent<Bullet>();
            bullet.Bullets = Bullets.transform.Find(data.value["id"].ToString());
            (bullet as RevolutionGem).entity = this;
            bullet.MaxPierce = GameManager.Inst.player.PiercingTarget;
            gems.Add(bullet as RevolutionGem);
        }
        if(revolutioning != null)
        {
            StopCoroutine(revolutioning);
        }
        revolutioning = StartCoroutine(Revolutioning());
    }
    public IEnumerator Revolutioning()
    {
        while (true)
        {
            if (Evo)
            {
                if (!Hitted)
                {
                    RevSpeed -= ItemManager.ConvertJToken<float>(data.value["val0"])[3] * 0.01f;
                }
                else
                {
                    Hitted = false;
                }
            }
            yield return d;
        }
    }
    private void FireReflectingGem()
    {
        lasers.Clear();
        RCount = Mathf.Min(10, (int)(((delay * GameManager.Inst.player.DelayRatio) + GameManager.Inst.player.NumericalDelay) * ((GameManager.Inst.player.BulletSpeedRatio * RevSpeed * speed) + GameManager.Inst.player.NumericalBulletSpeed)) / 3);
        rGems = new ReflectingGem[RCount];
        for (int i = 0; i < RCount; i++)
        {
            Transform gem = Bullets.transform.Find("ReflectingGem").GetChild(0);
            rGems[i] = gem.GetComponent<ReflectingGem>();
            rGems[i].spawner = this;
            gem.SetParent(null);
            gem.transform.position = transform.position;
            rGems[i].euler = Quaternion.Euler(0, 0, 360f / RCount * i + Random.Range(-(360f / RCount * 2), 360f / RCount * 2));
            gem.gameObject.SetActive(true);
        }
        reflect = StartCoroutine(Reflect());
    }
    private IEnumerator GatherReflectingGem()
    {
        foreach(ReflectingGem reflectingGem in rGems)
        {
            reflectingGem.gameObject.SetActive(false);
            reflectingGem.transform.SetParent(Bullets.transform.Find("ReflectingGem"));
        }
        if (reflect == null)
            yield break;
        StopCoroutine(reflect);
        reflect = null;
        if (lasers.Count == 0)
            yield break;
        Reflecting = false;
    }
    private IEnumerator Reflect()
    {
        Transform lParent = Bullets.transform.Find("GemLaser");
        //보석 부착 확인
        bool[] adhesion = new bool[RCount];
        while (System.Array.IndexOf(adhesion, false) > -1)
        {
            for (int i = 0; i < RCount; i++)
            {
                if (rGems[i].adhesion)
                {
                    adhesion[i] = true;
                }
            }
            yield return null;
        }
        //레이저 소환
        for (int i = 0; i < rGems.Length; i++)
        {
            for (int j = i + 1; j < rGems.Length; j++)
            {
                GameObject l = lParent.GetChild(0).gameObject;
                lasers.Add(l.GetComponent<GemLaser>());
                l.transform.SetParent(null);
                Vector3 sender = rGems[i].transform.position;
                Vector3 receiver = rGems[j].transform.position;
                l.transform.localScale = new(1f, Vector2.Distance(sender, receiver));
                l.transform.SetPositionAndRotation(rGems[i].transform.position, Quaternion.Euler(0, 0, Mathf.Atan2(sender.y - receiver.y, sender.x - receiver.x) * Mathf.Rad2Deg - 90));
                l.SetActive(true);
            }
        }
        Reflecting = true;
        //레이저 생성 반복
        while (Reflecting)
        {
            yield return null;
        }
        //종료
    }
}
