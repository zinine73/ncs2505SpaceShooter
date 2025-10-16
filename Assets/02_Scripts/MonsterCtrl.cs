using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Navigation
using UnityEngine.AI;
public class MonsterCtrl : MonoBehaviour
{
    const float TIME_WAIT = 0.3f;
    const int MAX_HP = 100;
    const int DAMAGE = 10;

    // 몬스터의 상태 정보
    public enum State { IDLE, TRACE, ATTACK, DIE }
    public State state = State.IDLE;
    public float traceDist = 10.0f;
    public float attackDist = 2.0f;
    public bool isDie = false;
    [SerializeField] CapsuleCollider body;
    [SerializeField] SphereCollider[] punch;

    // Animator parameter Hash값 추출
    readonly int hashTrace = Animator.StringToHash("IsTrace");
    readonly int hashAttack = Animator.StringToHash("IsAttack");
    readonly int hashHit = Animator.StringToHash("Hit");
    readonly int hashPlayerDie = Animator.StringToHash("PlayerDie");
    readonly int hashSpeed = Animator.StringToHash("Speed");
    readonly int hashDie = Animator.StringToHash("Die");

    Transform monsterTr;
    Transform playerTr;
    NavMeshAgent agent;
    Animator anim;
    GameObject bloodEffect; // 혈흔 효과 프리팹
    int hp = MAX_HP;

    void OnEnable() // 스크립트가 활성화 될 때
    {
        // 이벤트 발생 시 수행할 함수 연결
        PlayerCtrl.OnPlayerDie += OnPlayerDie;  

        // 몬스터의 상태를 체크하는 코루틴
        StartCoroutine(CheckMonsterState());
        // 상태에 따라 몬스터의 행동을 수행하는 코루틴
        StartCoroutine(MonsterAction());  
    }

    void OnDisable() // 스크립트가 비활성화 될 때
    {
        // 기존에 연결된 함수 해제
        PlayerCtrl.OnPlayerDie -= OnPlayerDie;
    }

    void Awake()
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
    }

    IEnumerator CheckMonsterState()
    {
        while (!isDie)
        {
            yield return new WaitForSeconds(TIME_WAIT);
            // 몬스터의 상태가 DIE일 때 코루틴 종료
            if (state == State.DIE) yield break;

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
                    isDie = true;
                    agent.isStopped = true;
                    anim.SetTrigger(hashDie);
                    
                    // 몬스터의 Collier 비활성화
                    EnableDisableCollider(false);
                    // 일정 시간 대기 후 오브젝트 풀링으로 환원
                    yield return new WaitForSeconds(3.0f);
                    // 사망 후 다시 사용하기 위한 초기화
                    hp = MAX_HP;
                    isDie = false;
                    EnableDisableCollider(true);
                    state = State.IDLE;                    
                    // 몬스터 비활성화
                    this.gameObject.SetActive(false);
                    break;
            }
            yield return new WaitForSeconds(TIME_WAIT);
        }
    }

    void EnableDisableCollider(bool value)
    {
        // body
        body.enabled = value;
        // punch
        foreach (var item in punch)
        {
            item.enabled = value;
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
            // 몬스터의 hp 차감
            hp -= DAMAGE;
            if (hp <= 0)
            {
                state = State.DIE;
            }
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
        if (state != State.DIE)
        {
            anim.SetFloat(hashSpeed, Random.Range(0.8f, 1.2f));
            anim.SetTrigger(hashPlayerDie);
        }
    }
}
