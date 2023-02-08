using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeddyWhale : Boss
{
    [SerializeField]
    private GameObject Water;
    [SerializeField]
    private GameObject waterPillar;
    [SerializeField]
    private GameObject waterDrop;
    [SerializeField]
    private GameObject teddywhaleBubble;
    private bool dive = false;
    protected override void FixedUpdate()
    {

    }
    protected override IEnumerator Spawned()
    {
        yield return StartCoroutine(base.Spawned());
        yield return new WaitForSeconds(1.5f);
        StartCoroutine(Pattern1());
    }
    private IEnumerator Pattern1()
    {
        WaitForSeconds onesec = new(1f);

        //경고 박스 생성 및 위치 조정
        GameObject caution = new()
        {
            tag = "EnemyChild"
        };
        SpriteRenderer caus = caution.AddComponent<SpriteRenderer>();
        caus.sprite = CautionBox;

        Vector3 plPos = GameManager.Inst.player.transform.position;
        Vector3 thisPos = transform.position;
        float angle = Mathf.Atan2(thisPos.y - plPos.y, thisPos.x - plPos.x) * Mathf.Rad2Deg;
        float dis = Vector2.Distance(transform.position, GameManager.Inst.player.transform.position);
        caution.transform.position = transform.position;
        caution.transform.localScale = new(7.5f, dis * 3.5f);
        caution.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);

        //1초간 대기
        yield return onesec;
        StopCoroutine(idle);
        Destroy(caution);

        //돌진
        while((transform.position - plPos).sqrMagnitude > 0.01f)
        {
            transform.position = Vector2.Lerp(transform.position, plPos, Time.deltaTime * MoveSpeed);
            yield return null;
        }
        transform.position = plPos;

        idle = StartCoroutine(Idle());
        //1초간 대기
        yield return onesec;

        //패턴 2 시작
        StartCoroutine(Pattern2());
    }
    private IEnumerator Pattern2()
    {
        WaitForSeconds onesec = new(1f);
        StopCoroutine(idle);
        //초기 위치로 이동
        while ((transform.position - new Vector3(0, 2.5f)).sqrMagnitude > 0.01f)
        {
            transform.position = Vector2.Lerp(transform.position, new(0, 2.5f), Time.deltaTime * MoveSpeed);
            yield return null;
        }
        transform.position = new(0, 2.5f);
        idle = StartCoroutine(Idle());
        
        //0.5초간 대기
        yield return new WaitForSeconds(0.5f);

        StopCoroutine(idle);

        //화면에서 사라짐
        while ((transform.position - new Vector3(0, 8f)).sqrMagnitude > 0.01f)
        {
            transform.position = Vector2.Lerp(transform.position, new(0, 8f), Time.deltaTime * MoveSpeed * 1.4f);
            yield return null;
        }
        transform.position = new(0, 8f);
        triggerCol.enabled = false;

        //경고 및 물 생성
        GameObject caution = new()
        {
            tag = "EnemyChild"
        };
        caution.AddComponent<SpriteRenderer>().sprite = CautionBox;
        caution.transform.localScale = new(32, 8);
        caution.transform.position = new(0, -2);

        GameObject water = Instantiate(Water);
        water.GetComponent<Water>().parent = this;
        water.transform.localScale = new(1, 0);
        water.transform.position = new(0, -5f);
        yield return onesec;

        //물 차오르기 시작
        caution.transform.localScale = new(0, 0);
        while(Mathf.Abs(water.transform.localScale.y - 1f) > 0.001f)
        {
            water.transform.localScale = new(1, Mathf.Lerp(water.transform.localScale.y, 1f, Time.deltaTime * MoveSpeed * 0.6f));
            yield return null;
        }
        water.transform.localScale = new(1, 1f);
        yield return onesec;

        //낙하 준비 및 경고 생성
        GameObject caution2 = new()
        {
            tag = "EnemyChild"
        };
        caution2.AddComponent<SpriteRenderer>().sprite = CautionBox;
        caution2.transform.localScale = new(19, 19);
        caution2.transform.position = new(0, 0.8f);

        float randX = Random.Range(-2.25f, 2.25f);
        caution.transform.localScale = new(7.5f, 50);
        caution.transform.position = new(randX, 10);
        transform.position = new(randX, 8f);
        yield return onesec;

        //낙하
        triggerCol.enabled = true;
        Destroy(caution);
        Destroy(caution2);
        transform.rotation = Quaternion.Euler(0, 0, 24);
        bool tempBool = false;
        while (Mathf.Abs(transform.position.y + 2.3f) > 0.01f)
        {
            transform.position = Vector2.Lerp(transform.position, new(randX, -2.3f), Time.deltaTime * MoveSpeed * 1.6f);
            if(!tempBool && transform.position.y < -1f)
            {
                tempBool = true;
                for (int i = 0; i < 20; i++)
                {
                    GameObject obj = Instantiate(waterDrop);
                    obj.GetComponent<WaterDrop>().parent = this;
                    obj.transform.position = new(transform.position.x, -2.3f);
                }
            }
            yield return null;
        }
        transform.position = new(transform.position.x, -2.3f);
        StartCoroutine(Pattern5());
    }
    private IEnumerator Pattern3()
    {
        idle = StartCoroutine(Idle());
        WaitForSeconds onesec = new(1f);
        yield return onesec;

        //경고 표시
        GameObject[] caution = new GameObject[3];
        GameObject[] WaterPillars = new GameObject[3];
        for (int i = 0; i<3; i++)
        {
            caution[i] = new()
            {
                tag = "EnemyChild"
            };
            caution[i].AddComponent<SpriteRenderer>().sprite = CautionBox;
            caution[i].transform.localScale = new(2, 100);
            caution[i].transform.position = new(Random.Range(-2.55f, 2.55f), 5);
        }

        yield return onesec;

        //물기둥 생성
        for (int i = 0; i < 3; i++)
        {
            WaterPillars[i] = Instantiate(waterPillar);
            WaterPillars[i].GetComponent<WaterPillar>().parent = this;
            WaterPillars[i].transform.localScale = new(0.5f, 1f);
            WaterPillars[i].transform.position = new(caution[i].transform.position.x, 0);
            Destroy(caution[i]);
        }

        yield return onesec;
        var c =WaterPillars[0].GetComponent<SpriteRenderer>();
        var _c =WaterPillars[1].GetComponent<SpriteRenderer>();
        var __c =WaterPillars[2].GetComponent<SpriteRenderer>();
        float a = 1;
        while (c.color.a > 0)
        {
            a -= Time.deltaTime * 2f;
            c.color = new(1, 1, 1, a);
            _c.color = new(1, 1, 1, a);
            __c.color = new(1, 1, 1, a);
            yield return null;
        }

        Destroy(WaterPillars[0]);
        Destroy(WaterPillars[1]);
        Destroy(WaterPillars[2]);

        yield return new WaitForSeconds(2f);
        StartCoroutine(Pattern4());
    }
    private IEnumerator Pattern4()
    {
        //거품 발사
        for(int i = 0; i<3; i++)
        {
            float randX = Random.Range(-2.5f, 2.5f);
            transform.rotation = (randX - transform.position.x) > 1 ? Quaternion.Euler(0, -180, transform.rotation.z) : Quaternion.Euler(0, 0, transform.rotation.z);
            while (Mathf.Abs(transform.position.x - randX) > 0.001f)
            {
                transform.position = new Vector3(Mathf.Lerp(transform.position.x, randX, Time.deltaTime * MoveSpeed), transform.position.y, transform.position.z);
                yield return null;
            }
            GameObject obj = Instantiate(teddywhaleBubble);
            obj.transform.position = transform.position;
            yield return new WaitForSeconds(1f);
        }
        yield return new WaitForSeconds(3f);
        StartCoroutine(Pattern5());
    }
    private IEnumerator Pattern5()
    {
        //물 상승 경고
        Water water = FindObjectOfType<Water>();

        GameObject caution = new()
        {
            tag = "EnemyChild"
        };
        caution.AddComponent<SpriteRenderer>().sprite = CautionBox;
        caution.transform.localScale = new(32, 12);
        caution.transform.position = new(0, -2);

        yield return new WaitForSeconds(1f);

        Destroy(caution);

        //물 상승 및 투명화
        SpriteRenderer waterSprite = water.GetComponent<SpriteRenderer>();
        while (1.5f - water.transform.localScale.y> 0.001f)
        {
            float sizeY = Mathf.Lerp(water.transform.localScale.y, 1.5f, Time.deltaTime * 5f);
            water.transform.localScale = new(1, sizeY);
            waterSprite.color = new(1, 1, 1, Mathf.Max(0.8f, waterSprite.color.a - Time.deltaTime));
            transform.position = new(transform.position.x, Mathf.Lerp(transform.position.y, -1.2f, Time.deltaTime * 5));
            yield return null;
        }
        water.transform.localScale = new(1, 1.5f);
        transform.position = new(transform.position.x, -1.2f);

        yield return new WaitForSeconds(1f);

        //무작위 위치 잠수 및 점프
        float randX = Random.Range(-2.5f, 2.5f);
        transform.rotation = randX > 1 ? Quaternion.Euler(0, -180, 108) : Quaternion.Euler(0, 0, 108);
        StartCoroutine(DiveAndJump());
        while(dive)
        {
            transform.Translate(Time.deltaTime * new Vector2(Mathf.Sign(randX), 0));
            transform.position = new(Mathf.Clamp(transform.position.x, -2.5f, 2.5f), transform.position.y);
            yield return null;
        }
        //물 삭제

        while (water.transform.localScale.y > 0.01f)
        {
            float sizeY = Mathf.Lerp(water.transform.localScale.y, 0, Time.deltaTime * 5f);
            water.transform.localScale = new(1, sizeY);
            yield return null;
        }
        Destroy(water.gameObject);
        StopCoroutine(idle);
        transform.rotation = Quaternion.Euler(0, 0, 0);
        while ((transform.position - new Vector3(0, 2.5f)).sqrMagnitude > 0.01f)
        {
            transform.position = Vector2.Lerp(transform.position, new(0, 2.5f), Time.deltaTime * MoveSpeed);
            yield return null;
        }
        transform.position = new(0, 2.5f);
        idle = StartCoroutine(Idle());
        yield return new WaitForSeconds(3f);
    }
    private IEnumerator DiveAndJump()
    {
        dive = true;
        //잠수
        while(transform.position.y > -3f)
        {
            Debug.Log("잠수");
            transform.Translate(MoveSpeed * Time.deltaTime * Vector2.down);
            yield return null;
        }
        //점프
        float acc = 1f;
        while(transform.position.y < 3f)
        {
            Debug.Log("점프");
            transform.Translate(MoveSpeed * Time.deltaTime * acc * Vector2.up);
            acc += Time.deltaTime;
            transform.rotation = Quaternion.Euler(0, transform.rotation.y, Mathf.Lerp(transform.rotation.z, -25f, Time.deltaTime));
            yield return null;
        }
        acc = 1f;
        //하강
        while(transform.position.y > -1.2f)
        {
            Debug.Log("하강");
            transform.Translate(MoveSpeed * Time.deltaTime * acc * Vector2.down);
            acc += Time.deltaTime;
            transform.rotation = Quaternion.Euler(0, transform.rotation.y, Mathf.Lerp(transform.rotation.z, 108f, Time.deltaTime));
            yield return null;
        }
        dive = false;
    }
    protected override void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!SpikeTimer)
            {
                SpikeTimer = true;
                HP += ChangeHS(true, GameManager.Inst.player.SpikeDmg);
                if (GameManager.Inst.player.itemEvos["SpikyBall"])
                {
                    GameObject obj = Instantiate((GameManager.Inst.player.Items["SpikyBall"] as SpikyBall).spikes);
                    obj.transform.position = transform.position;
                }
            }
            if (gameObject.activeSelf)
                GameManager.Inst.player.Hit(this, dmg);
        }
    }
}
