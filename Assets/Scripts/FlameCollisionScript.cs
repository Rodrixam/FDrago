using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameCollisionScript : MonoBehaviour
{
    ParticleSystem ps;
    List<ParticleSystem.Particle> enter;

    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
        enter = new List<ParticleSystem.Particle>();
    }

    void OnParticleTrigger()
    {
        // get the particles which matched the trigger conditions this frame
        int numEnter = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter, out ParticleSystem.ColliderData insideData);
        for(int i = 0; i < numEnter; i++)
        {
            int colCount = insideData.GetColliderCount(i);
            for(int a = 0; a < colCount; a++)
            {
                insideData.GetCollider(i,a).gameObject.GetComponent<EnemyController>().SetInsideFlames();
            }
        }
    }
}
