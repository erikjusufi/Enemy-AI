using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.FPS.Game;

public class EnemyAI : MonoBehaviour
{
    // target to catch
    [SerializeField] Transform target;
    [SerializeField] float chaseRange = 10;
    [SerializeField] float sight = 15;
    [SerializeField] float leapRange = 6;
    [SerializeField] float turnSpeed = 5;
    bool isScanning = false;
    

    public NavMeshAgent navMashAgent;

    float distanceToTarget = Mathf.Infinity;
    bool isProvoked = false;
    Health health;
    [SerializeField] LightAttack lightAttack;


    //patrol
    public float speed;
    private float waitTime;
    public float startWaitTime;
    public Transform[] moveSpots;
    private int randomSpot;

    void Start()
    {
        navMashAgent = GetComponent<NavMeshAgent>();
        health = GetComponent<Health>();
        waitTime = startWaitTime;
        randomSpot = UnityEngine.Random.Range(0, moveSpots.Length);
        
        
    }




    void Update()
    {
        if (isScanning)
        {
            lightAttack.HandleSwing();
            isScanning = false;
        }
        if(health.isDead())
        {
            enabled = false;
            navMashAgent.enabled = false;
            isProvoked = false;
        }
        else if(health.isReanim())
        {
            navMashAgent.enabled = true;
        }
        distanceToTarget = Vector3.Distance(target.position, transform.position);
        if (distanceToTarget > sight)
        {
            isProvoked = false;
            GetComponent<Animator>().SetBool("attack", false);

        }
        if (isProvoked)
        {
            GetComponent<Animator>().SetBool("idle", false);
            EngageTarget();


        }
        else if (distanceToTarget <= chaseRange)
        {
            isProvoked = true;
            GetComponent<Animator>().SetBool("idle", false);

        }
        else if(!isProvoked)
        {
            Patrol();
        }
        
            
    }
    private void Patrol()
    {

        GetComponent<Animator>().SetBool("idle",false);
        FaceSpot();
        transform.position = Vector3.MoveTowards(transform.position, moveSpots[randomSpot].position, speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, moveSpots[randomSpot].position) < 0.2f)
        {
            GetComponent<Animator>().SetBool("idle",true);
            if (waitTime <= 0)
            {
                randomSpot = UnityEngine.Random.Range(0, moveSpots.Length);
                waitTime = startWaitTime;
            }
            else
            {
                waitTime -= Time.deltaTime;
            }
        }
    }
    private void EngageTarget()
    {
        FaceTarget();
        if  (distanceToTarget < leapRange && distanceToTarget > navMashAgent.stoppingDistance+2 && !GetComponent<Animator>().GetBool("attack"))
        {
            Leap();
        }
        else if (distanceToTarget >= navMashAgent.stoppingDistance)
        {
            ChaseTarget();
        }
        else if(distanceToTarget < navMashAgent.stoppingDistance)
        {
            AttackTarget();
        }
    }

    private void AttackTarget()
    {
        GetComponent<Animator>().SetBool("leap", false);
        if (UnityEngine.Random.Range(1,10) < 5)
        {
            GetComponent<Animator>().SetBool("heavy",true);
        }
        else
        {
            GetComponent<Animator>().SetBool("heavy", false);
            GetComponent<Animator>().SetBool("attack", true);
        }
        
    }
    private void Leap()
    {
        GetComponent<Animator>().SetBool("leap",true);
        GetComponent<Animator>().SetBool("attack", true);
    }
    private void ChaseTarget()
    {
        GetComponent<Animator>().SetBool("heavy", false);
        GetComponent<Animator>().SetBool("attack", false);
        GetComponent<Animator>().SetBool("leap", false);
        GetComponent<Animator>().SetTrigger("move");
        navMashAgent.SetDestination(target.position);
    }
    private void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, direction.y, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);
    }
    private void FaceSpot()
    {
        Vector3 direction = (moveSpots[randomSpot].position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, direction.y, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
    }

    private void AttackStarted()
    {
        //Debug.Log("attack started");
        
        isScanning = true;
    }

    private void AttackEnded()
    {
        //Debug.Log("attack ended");
        isScanning = false;
    }
}
