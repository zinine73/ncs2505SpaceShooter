using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Navigation
using UnityEngine.AI;
public class MonsterCtrl : MonoBehaviour
{
    const float TIME_WAIT = 0.3f;

    // 몬스터의 상태 정보
    public enum State { IDLE, TRACE, ATTACK, DIE }
    public State state = State.IDLE;
    public float traceDist = 10.0f;
    public float attackDist = 2.0f;
    public bool isDie = false;

    Transform monsterTr;
    Transform playerTr;
    NavMeshAgent agent;
    Animator anim;
    void Start()
    {
        // monsterTr
        monsterTr = GetComponent<Transform>();
        // playerTr
        playerTr = GameObject.FindWithTag("PLAYER")
            .GetComponent<Transform>();
        // agent
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        // start trace
        //agent.destination = playerTr.position;
        //agent.SetDestination(playerTr.position);

        // 몬스터의 상태를 체크하는 코루틴
        StartCoroutine(CheckMonsterState());
        // 상태에 따라 몬스터의 행동을 수행하는 코루틴
        StartCoroutine(MonsterAction());
    }

    IEnumerator CheckMonsterState()
    {
        while (!isDie)
        {
            yield return new WaitForSeconds(TIME_WAIT);
            float distance = Vector3.Distance(playerTr.position, monsterTr.position);
            if (distance <= attackDist)
            {
                state = State.ATTACK;
            }
            else if (distance <= traceDist)
            {
                state = State.TRACE;
            }
            else
            {
                state = State.IDLE;
            }
        }
    }

    IEnumerator MonsterAction()
    {
        while (!isDie)
        {
            switch (state)
            {
                case State.IDLE:
                    agent.isStopped = true;
                    anim.SetBool("IsTrace", false);
                    break;
                case State.TRACE:
                    agent.SetDestination(playerTr.position);
                    agent.isStopped = false;
                    anim.SetBool("IsTrace", true);
                    break;
                case State.ATTACK:
                    break;
                case State.DIE:
                    break;
            }
            yield return new WaitForSeconds(TIME_WAIT);
        }
    }

    void OnDrawGizmos()
    {
        if (state == State.TRACE)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, traceDist);
        }
        if (state == State.ATTACK)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackDist);
        }
    }
}
