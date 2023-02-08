using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CC
{
    public string caster;
    public Coroutine co = null;
    private Enemy obj;
    public string type;
    public object value;
    public float time;
    public CC(string caster, string type, object value, float time, Enemy obj)
    {
        this.obj = obj;
        this.caster = caster;
        this.type = type;
        this.value = value;
        this.time = time;
        SetCC();
    }
    private void SetCC()
    {
        if (obj == null)
        {
            Player target = GameManager.Inst.player;
            switch (type)
            {
                case "slow":
                    target.MoveSpeed *= 1 - Mathf.Min((float)value, 0.99f);
                    break;
                case "vulnerable":
                    target.Vulnerable *= 1 + (float)value;
                    break;
                case "weakness":
                    target.BulletDmgRatio *= (1 - (float)value);
                    break;
                case "stun":
                    target.stopped = true;
                    break;
                case "poison":
                    target.SetDot(this);
                    target.MicsEffect.Find("Poison").gameObject.SetActive(true);
                    break;
                case "bleed":
                    target.SetDot(this);
                    target.MicsEffect.Find("Bleed").gameObject.SetActive(true);
                    break;
                case "grabbed":
                    target.grab = true;
                    target.grabbed = (Vector2)value;
                    break;
                case "dizziness":
                    target.Dizziness = true;
                    break;
                case "laziness":
                    target.DelayRatio /= 1 + (float)value;
                    break;
                case "drained":
                    target.Drained /= 1 + (float)value;
                    break;
            }
            target.MicsEffect.Find(type)?.gameObject.SetActive(true);
        }
        else
        {
            Enemy target = obj;
            switch (type)
            {
                case "slow":
                    target.MoveSpeed *= 1 - Mathf.Min((float)value, 0.99f);
                    break;
                case "vulnerable":
                    target.Vulnerable *= 1 + (float)value;
                    break;
                case "weakness":
                    target.dmg *= (1 - (float)value);
                    break;
                case "stun":
                    target.stopped = true;
                    break;
                case "poison":
                    target.SetDot(this);
                    break;
                case "bleed":
                    target.SetDot(this);
                    break;
                case "grabbed":
                    target.grab = true;
                    target.grabbed = (Vector2)value;
                    break;
            }
            obj.MicsEffect.Find(type)?.gameObject.SetActive(true);
        }
    }
    public IEnumerator ApplyCC()
    {
        while(time > 0)
        {
            if(obj == null)
            {
                GameManager.Inst.player.CCAccum[type] += Time.deltaTime;
            }
            else
            {
                obj.CCAccum[type] += Time.deltaTime;
            }
            time -= Time.deltaTime;
            yield return null;
        }
        Remove();
    }
    public void Remove()
    {
        bool effectOn = false;
        if (obj == null)
        {
            Player target = GameManager.Inst.player;
            switch (type)
            {
                case "slow":
                    target.MoveSpeed /= 1 - Mathf.Min((float)value, 0.99f);
                    break;
                case "vulnerable":
                    target.Vulnerable /= 1 + (float)value;
                    break;
                case "weakness":
                    target.BulletDmgRatio /= (1 - (float)value);
                    break;
                case "stun":
                    target.stopped = false;
                    break;
                case "poison":
                    target.StopDot(this);
                    break;
                case "bleed":
                    target.StopDot(this);
                    break;
                case "grabbed":
                    target.grab = false;
                    break;
                case "dizziness":
                    target.Dizziness = false;
                    break;
                case "laziness":
                    target.DelayRatio *= 1 + (float)value;
                    break;
                case "drained":
                    target.Drained *= 1 + (float)value;
                    break;
            }
            target.CC.Remove(this);

            foreach (CC cC in target.CC)
            {
                if (cC.type == type)
                {
                    effectOn = true;
                    break;
                }
            }

            target.MicsEffect.Find(type)?.gameObject.SetActive(effectOn);
        }
        else
        {
            Enemy target = obj;
            switch (type)
            {
                case "slow":
                    target.MoveSpeed /= 1 - Mathf.Min((float)value, 0.99f);
                    break;
                case "vulnerable":
                    target.Vulnerable /= 1 + (float)value;
                    break;
                case "weakness":
                    target.dmg /= (1 - (float)value);
                    break;
                case "stun":
                    target.stopped = false;
                    break;
                case "poison":
                    target.StopDot(this);
                    break;
                case "bleed":
                    target.StopDot(this);
                    break;
                case "grabbed":
                    target.grab = false;
                    break;
            }
            target.CC.Remove(this);

            foreach (CC cC in obj.CC)
            {
                if(cC.type == type)
                {
                    effectOn = true;
                    break;
                }
            }

            obj.MicsEffect.Find(type)?.gameObject.SetActive(effectOn);
        }
    }
}
