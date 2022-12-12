using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeddyWhale : Boss
{
    [SerializeField]
    private GameObject Water;
    [SerializeField]
    private GameObject waterPillar;
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
        GameObject caution = new();
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
            transform.position = Vector2.Lerp(transform.position, plPos, Time.deltaTime * 5f);
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
            transform.position = Vector2.Lerp(transform.position, new(0, 2.5f), Time.deltaTime * 5f);
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
            transform.position = Vector2.Lerp(transform.position, new(0, 8f), Time.deltaTime * 7f);
            yield return null;
        }
        transform.position = new(0, 8f);

        //경고 및 물 생성
        GameObject caution = new();
        caution.AddComponent<SpriteRenderer>().sprite = CautionBox;
        caution.transform.localScale = new(32, 8);
        caution.transform.position = new(0, -2);

        GameObject water = Instantiate(Water);
        water.transform.localScale = new(1, 0);
        water.transform.position = new(0, -5f);
        yield return onesec;

        //물 차오르기 시작
        caution.transform.localScale = new(0, 0);
        while(Mathf.Abs(water.transform.localScale.y - 0.25f) > 0.001f)
        {
            water.transform.localScale = new(1, Mathf.Lerp(water.transform.localScale.y, 0.25f, Time.deltaTime * 3f));
            yield return null;
        }
        water.transform.localScale = new(1, 0.25f);
        yield return onesec;

        //낙하 준비 및 경고 생성
        GameObject caution2 = new();
        caution2.AddComponent<SpriteRenderer>().sprite = CautionBox;
        caution2.transform.localScale = new(19, 19);
        caution2.transform.position = new(0, 0.8f);

        float randX = Random.Range(-2.25f, 2.25f);
        caution.transform.localScale = new(7.5f, 50);
        caution.transform.position = new(randX, 10);
        transform.position = new(randX, 8f);
        yield return onesec;

        //낙하
        Destroy(caution);
        Destroy(caution2);
        transform.rotation = Quaternion.Euler(0, 0, 24);
        water.transform.GetChild(0).localPosition = new(randX, 10f);
        while (Mathf.Abs(transform.position.y + 2f) > 0.01f)
        {
            transform.position = Vector2.Lerp(transform.position, new(randX, -2f), Time.deltaTime * 8f);
            if(transform.position.y < -1f && !water.transform.GetChild(0).GetComponent<ParticleSystem>().isPlaying)
            {
                water.transform.GetChild(0).GetComponent<ParticleSystem>().trigger.AddCollider(GameManager.Inst.player.transform);
                water.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
            }
            yield return null;
        }
        Destroy(water.transform.GetChild(0).gameObject, 1f);

        StartCoroutine(Pattern3());
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
            caution[i] = new();
            caution[i].AddComponent<SpriteRenderer>().sprite = CautionBox;
            caution[i].transform.localScale = new(2, 100);
            caution[i].transform.position = new(Random.Range(-2.55f, 2.55f), 5);
        }

        yield return onesec;

        //물기둥 생성
        for (int i = 0; i < 3; i++)
        {
            WaterPillars[i] = Instantiate(waterPillar);
            WaterPillars[i].transform.localScale = new(0.5f, 10);
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

        Water water = FindObjectOfType<Water>();
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
            transform.position = Vector2.Lerp(transform.position, new(0, 2.5f), Time.deltaTime * 5f);
            yield return null;
        }
        transform.position = new(0, 2.5f);
        idle = StartCoroutine(Idle());
        yield return new WaitForSeconds(3f);
        StartCoroutine(Pattern1());
    }
}
