using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenWatchSpawner : BulletSpawner
{
    private WaitForSeconds paradeTime;
    public int SetCard = 0;// 0 : AllRandom, 1 : BlackCard, 2 : RedCard
    public GameObject cardclover, cloverExplosion, carddiamond, cardspade, cardheart;
    public GameObject RedPortal, BlackPortal;
    public GameObject RedParade, BlackParade;
    private int shootCount;

    protected override void Awake()
    {
        base.Awake();
        paradeTime = new((float)(data.value["val1"] as Newtonsoft.Json.Linq.JToken)[2]);
    }
    protected override void PoolingBullet()
    {
        base.PoolingBullet();

        if (!Bullets.transform.Find("CardSpade"))
        {
            GameObject cardspade_parent = new();
            cardspade_parent.transform.SetParent(Bullets.transform);
            cardspade_parent.name = "CardSpade";
            for (int i = 0; i < (int)data.value["limit"]; i++)
            {
                GameObject obj = Instantiate(cardspade, cardspade_parent.transform);
                obj.GetComponent<Bullet>().entity = this;
                obj.name = "CardSpade";
                obj.SetActive(false);
            }
        }

        if (!Bullets.transform.Find("CardHeart"))
        {
            GameObject cardheart_parent = new();
            cardheart_parent.transform.SetParent(Bullets.transform);
            cardheart_parent.name = "CardHeart";
            for (int i = 0; i < (int)data.value["limit"]; i++)
            {
                GameObject obj = Instantiate(cardheart, cardheart_parent.transform);
                obj.GetComponent<Bullet>().entity = this;
                obj.name = "CardHeart";
                obj.SetActive(false);
            }
        }

        if (!Bullets.transform.Find("CardDiamond"))
        {
            GameObject carddiamond_parent = new();
            carddiamond_parent.transform.SetParent(Bullets.transform);
            carddiamond_parent.name = "CardDiamond";
            for (int i = 0; i < (int)data.value["limit"]; i++)
            {
                GameObject obj = Instantiate(carddiamond, carddiamond_parent.transform);
                obj.GetComponent<Bullet>().entity = this;
                obj.name = "CardDiamond";
                obj.SetActive(false);
            }
        }

        if (!Bullets.transform.Find("CardClover"))
        {
            GameObject cardclover_parent = new();
            cardclover_parent.transform.SetParent(Bullets.transform);
            cardclover_parent.name = "CardClover";
            for (int i = 0; i < (int)data.value["limit"]; i++)
            {
                GameObject obj = Instantiate(cardclover, cardclover_parent.transform);
                obj.GetComponent<Bullet>().entity = this;
                obj.name = "CardClover";
                obj.SetActive(false);
            }
        }

        if (!Bullets.transform.Find("CloverExplosion"))
        {
            GameObject cardcloverex_parent = new();
            cardcloverex_parent.transform.SetParent(Bullets.transform);
            cardcloverex_parent.name = "CloverExplosion";
            for (int i = 0; i < (int)data.value["limit"]; i++)
            {
                GameObject obj = Instantiate(cloverExplosion, cardcloverex_parent.transform);
                obj.GetComponent<Bullet>().entity = this;
                obj.name = "CloverExplosion";
                obj.SetActive(false);
            }
        }
    }
    private IEnumerator SpawnBlackParade()
    {
        SetCard = 1;
        delay *= 1 + ((float)data.value["val3"] * 0.01f);
        GameManager.Inst.player.GetCC("BlackParade", "grabbed", Vector2.zero, 2f);
        yield return paradeTime;
        delay /= 1 + ((float)data.value["val3"] * 0.01f);
        SetCard = 0;
        GameManager.Inst.player.GetCC("BlackParade", "grabbed", Vector2.zero, 2f);
        if (FindObjectOfType<Carrot>().Evo)
        {
            FindObjectOfType<Carrot>().SpawnBlackCarrot();
        }
    }
    private IEnumerator SpawnRedParade()
    {
        if (FindObjectOfType<Carrot>().Evo)
        {
            FindObjectOfType<Carrot>().SpawnRedCarrot();
        }
        SetCard = 2;
        dmg *= 1 + ((float)data.value["val4"] * 0.01f);
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach(GameObject enemy in enemies)
        {
            enemy.GetComponent<Enemy>().GetCC("RedParade", "grabbed", Vector2.zero, 2f);
        }
        yield return paradeTime;
        dmg /= 1 + ((float)data.value["val4"] * 0.01f);
        SetCard = 0;
    }
    private IEnumerator BlackJokerCard()
    {
        int i = 0;
        yield return new WaitForSeconds(0.5f);
        foreach (NormalEnemy e in FindObjectsOfType<NormalEnemy>())
        {
            if (e.HP < e.maxhp * ItemManager.ConvertJToken<float>(data.value["val1"])[0] * 0.01f)
            {
                e.HP = 0;
                i++;
            }
        }
        if(i >= ItemManager.ConvertJToken<int>(data.value["val1"])[1])
        {
            StartCoroutine(SpawnBlackParade());
        }
    }
    private IEnumerator RedJokerCard()
    {
        int i = 0;
        yield return new WaitForSeconds(0.5f);
        foreach (NormalEnemy e in FindObjectsOfType<NormalEnemy>())
        {
            if (e.HP > e.maxhp * ItemManager.ConvertJToken<float>(data.value["val2"])[0] * 0.01f)
            {
                e.HP = 0;
                i++;
            }
        }
        if (i >= ItemManager.ConvertJToken<int>(data.value["val2"])[1])
        {
            StartCoroutine(SpawnRedParade());
        }
    }
    private void SpadeCard()
    {
        if (SetCard == 1)
        {
            if (Random.Range(0, 2) == 0)
            {
                CloverCard();
                return;
            }
        }
        else if (SetCard == 2)
        {
            if (Random.Range(0, 2) == 0)
            {
                HeartCard();
                return;
            }
            else
            {
                DiamondCard();
                return;
            }
        }
        ShootBullet(Bullets.transform.Find("CardSpade").GetChild(0).GetComponent<Bullet>(), Bullets.transform.Find("CardSpade"), "CardSpade");
    }
    private void CloverCard()
    {
        if (SetCard == 1)
        {
            if (Random.Range(0, 2) == 1)
            {
                SpadeCard();
                return;
            }
        }
        else if (SetCard == 2)
        {
            if (Random.Range(0, 2) == 0)
            {
                HeartCard();
                return;
            }
            else
            {
                DiamondCard();
                return;
            }
        }
        ShootBullet(Bullets.transform.Find("CardClover").GetChild(0).GetComponent<Bullet>(), Bullets.transform.Find("CardClover"), "CardClover");
    }
    private void HeartCard()
    {
        if (SetCard == 1)
        {
            if (Random.Range(0, 2) == 0)
            {
                CloverCard();
                return;
            }
            else
            {
                SpadeCard();
                return;
            }
        }
        else if (SetCard == 2)
        {
            if (Random.Range(0, 2) == 1)
            {
                DiamondCard();
                return;
            }
        }
        Transform pa = Bullets.transform.Find("CardHeart");
        ShootBullet(pa.GetChild(0).GetComponent<Bullet>(), pa, "CardHeart");

    }
    private void DiamondCard()
    {
        if (SetCard == 1)
        {
            if (Random.Range(0, 2) == 0)
            {
                CloverCard();
                return;
            }
            else
            {
                SpadeCard();
                return;
            }
        }
        else if (SetCard == 2)
        {
            if (Random.Range(0, 2) == 0)
            {
                HeartCard();
                return;
            }
        }
        Transform pa = Bullets.transform.Find("CardDiamond");
        ShootBullet(pa.GetChild(0).GetComponent<Bullet>(), pa, "CardDiamond");
    }
    protected override void ShootBullet()
    {
        shootCount++;
        if(shootCount == 3 || shootCount == 15)
        {
            CloverCard();
            return;
        }
        else if(shootCount == 6 || shootCount == 18)
        {
            DiamondCard();
            return;
        }
        else if(shootCount == 9 || shootCount == 21)
        {
            HeartCard();
            return;
        }
        else if(shootCount == 12)
        {
            SpadeCard();
            return;
        }
        else if(shootCount == 24 && Evo)
        {
            SpadeCard();
            if (Evo)
            {
                if (Random.Range(0, 2) == 0)
                {
                    StartCoroutine(RedJokerCard());
                }
                else
                {
                    StartCoroutine(BlackJokerCard());
                }
            }
            shootCount = 0;
            return;
        }
        else
        {
            Transform pa = Bullets.transform.Find(data.value["id"].ToString());
            ShootBullet(pa.GetChild(0).GetComponent<Bullet>(), pa, data.value["id"].ToString());
        }
    }
}
