using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelCtrl : MonoBehaviour
{
    const int HIT_COUNT = 3; // 몇번 맞아야 폭발할지
    const float DESTROY_EXP = 5.0f; // 폭발효과 5초 후에 제거
    const float DESTROY_BARREL = 3.0f; // 3초 후에 Barrel 제거
    const float BARREL_MASS = 1.0f; // Barrel의 무게를 가볍게 함
    const float UP_FORCE = 1500.0f; // 위로 솟구치는 힘을 가함

    [SerializeField] GameObject expEffect;
    Transform tr;
    Rigidbody rb;
    int hitCount = 0;

    void Start()
    {
        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
    }

    // 충돌시 발생하는 콜백함수
    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("BULLET"))
        {
            if (++hitCount == HIT_COUNT)
            {
                ExpBarrel();
            }
        }
    }

    void ExpBarrel()
    {
        // 폭발효과 파티클 생성
        GameObject exp = Instantiate(expEffect,
            tr.position, Quaternion.identity);
        // 폭발효과 5초 후에 제거
        Destroy(exp, DESTROY_EXP);
        // Barrel의 무게를 가볍게 함
        rb.mass = BARREL_MASS;
        // 위로 솟구치는 힘을 가함
        rb.AddForce(Vector3.up * UP_FORCE);
        // 3초 후에 Barrel 제거
        Destroy(gameObject, DESTROY_BARREL);
    }
}
