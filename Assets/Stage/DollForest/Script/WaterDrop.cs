using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDrop : MonoBehaviour
{
    private void OnParticleTrigger()
    {
        ParticleSystem ps = GetComponent<ParticleSystem>();

        List<ParticleSystem.Particle> enter = new();

        int numEnter = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);

        if (numEnter>0 && !GameManager.Inst.player.Invincible)
        {
            GameManager.Inst.player.HP -= 8;
        }
    }
}
