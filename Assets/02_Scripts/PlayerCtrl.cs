using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    const float TIME_INTER = 0.25f;
    const float INPUT_VALUE = 0.1f;
    const float INIT_HP = 100.0f;
    const float PUNCH_POWER = 10.0f;

    // component cash
    Transform tr;
    Animation anim;
    float currHp;
    [SerializeField] float moveSpeed = 10.0f;
    [SerializeField] float turnSpeed = 80.0f;

    // delagate 선언
    public delegate void PlayerDieHandler();
    // event 선언
    public static event PlayerDieHandler OnPlayerDie;

    IEnumerator Start()
    {
        currHp = INIT_HP;
        // Get Component
        //tr = this.gameObject.GetComponent<Transform>();
        //tr = GetComponent("Transform") as Transform;
        //tr = (Transform)GetComponent(typeof(Transform));
        tr = GetComponent<Transform>();
        anim = GetComponent<Animation>();

        //anim.clip = anim.GetClip("Idle");
        //anim.Play();
        anim.Play("Idle");

        turnSpeed = 0.0f;
        yield return new WaitForSeconds(0.3f);
        turnSpeed = 80.0f;
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        float r = Input.GetAxis("Mouse X");
        // Transform.position
        //transform.position += new Vector3(0, 0, 1);
        // normalized vector
        //tr.position += Vector3.forward * 1;
        Vector3 moveDir = (Vector3.forward * v) + (Vector3.right * h);
        tr.Translate(moveDir.normalized * moveSpeed * Time.deltaTime);
        tr.Rotate(Vector3.up * turnSpeed * Time.deltaTime * r);
        PlayerAnim(h, v);
    }

    void PlayerAnim(float h, float v)
    {
        if (v >= INPUT_VALUE)
        {
            anim.CrossFade("RunF", TIME_INTER);
        }
        else if (v <= -INPUT_VALUE)
        {
            anim.CrossFade("RunB", TIME_INTER);
        }
        else if (h >= INPUT_VALUE)
        {
            anim.CrossFade("RunR", TIME_INTER);
        }
        else if (h < -INPUT_VALUE)
        {
            anim.CrossFade("RunL", TIME_INTER);
        }
        else
        {
            anim.CrossFade("Idle", TIME_INTER);
        }
    }

    void OnTriggerEnter(Collider coll)
    {
        if (currHp > 0.0f && coll.CompareTag("PUNCH"))
        {
            currHp -= PUNCH_POWER;
            //Debug.Log("Player HP = " + currHp / INIT_HP);
            //Debug.LogFormat("Player HP = {0}", currHp / INIT_HP);
            Debug.Log($"Player HP = {currHp / INIT_HP}");

            if (currHp <= 0.0f)
            {
                PlayerDie();
            }
        }
    }

    void PlayerDie()
    {
        Debug.Log("Player Die !");
        /*
        // MONSTER tag를 가진 모든 게임오브젝트를 찾기
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("MONSTER");
        // 모든 몬스터의 OnPlayerDie 함수를 순차적으로 호출
        foreach (var item in monsters)
        {
            item.SendMessage("OnPlayerDie", SendMessageOptions.DontRequireReceiver);
        }
        */
        // 주인공 사망 이벤트 호출(발생)
        OnPlayerDie();
    }
}
