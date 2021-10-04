using System.Collections;
using System.Collections.Generic;
using Unity.FPS.Game;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    Health health;
    EnemyAI enemyAI;
    float toStart = 7;
    void Start()
    {
        health = GetComponent<Health>();
        enemyAI = GetComponent<EnemyAI>();
    }
    void Update()
    {
        if(health.isReanim())
        {
            toStart = toStart - Time.deltaTime;
            if(toStart <= 0)
            {
                enemyAI.enabled = true;
                enemyAI.navMashAgent.enabled = true;
                toStart = 7;
                health.setReanim(false);
            }
                

        }
    }
}
