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

    // Animator parameter Hash값 추출
    readonly int hashTrace = Animator.StringToHash("IsTrace");
    readonly int hashAttack = Animator.StringToHash("IsAttack");
    readonly int hashHit = Animator.StringToHash("Hit");
    readonly int hashPlayerDie = Animator.StringToHash("PlayerDie");
    readonly int hashSpeed = Animator.StringToHash("Speed");

    Transform monsterTr;
    Transform playerTr;
    NavMeshAgent agent;
    Animator anim;
    GameObject bloodEffect; // 혈흔 효과 프리팹
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
        // bloodEffect prefab load
        bloodEffect = Resources.Load<GameObject>("BloodSprayEffect");

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
                    anim.SetBool(hashTrace, false);
                    break;
                case State.TRACE:
                    agent.SetDestination(playerTr.position);
                    agent.isStopped = false;
                    anim.SetBool(hashTrace, true);
                    anim.SetBool(hashAttack, false);
                    break;
                case State.ATTACK:
                    anim.SetBool(hashAttack, true);
                    break;
                case State.DIE:
                    break;
            }
            yield return new WaitForSeconds(TIME_WAIT);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("BULLET"))
        {
            // 총알 삭제
            Destroy(collision.gameObject);
            // 피격 에니메이션 실행
            anim.SetTrigger(hashHit);
            // 총알의 충돌지점
            Vector3 pos = collision.GetContact(0).point;
            Quaternion rot = Quaternion.LookRotation(-collision.GetContact(0).normal);
            // bloodEffect 발생
            ShowBloodEffect(pos, rot);
        }
    }

    void ShowBloodEffect(Vector3 pos, Quaternion rot)
    {
        GameObject blood = Instantiate<GameObject>(bloodEffect, pos, rot, monsterTr);
        Destroy(blood, 1.0f);
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

    // void OnTriggerStay(Collider other)
    // {
    //     Debug.Log(other.gameObject.name);
    // }

    void OnPlayerDie()
    {
        // 몬스터의 상태를 체크하는 코루틴 함수를 정지
        StopAllCoroutines();
        // 추적을 정지하고 애니메이션을 수행
        agent.isStopped = true;
        anim.SetFloat(hashSpeed, Random.Range(0.8f, 1.2f));
        anim.SetTrigger(hashPlayerDie);
    }
}
