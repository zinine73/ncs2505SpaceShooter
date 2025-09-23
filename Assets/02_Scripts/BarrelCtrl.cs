using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BarrelCtrl : MonoBehaviour
{
    const int HIT_COUNT = 3; // 몇번 맞아야 폭발할지
    const float DESTROY_EXP = 5.0f; // 폭발효과 5초 후에 제거
    const float DESTROY_BARREL = 3.0f; // 3초 후에 Barrel 제거
    const float BARREL_MASS = 1.0f; // Barrel의 무게를 가볍게 함
    const float UP_FORCE = 1500.0f; // 위로 솟구치는 힘을 가함
    const float OVER_FORCE = 1200.0f;

    [SerializeField] Transform barrelEffectTr;
    [SerializeField] GameObject expEffect;
    [SerializeField] Texture[] textures;
    [SerializeField] float radius = 10.0f;
    new MeshRenderer renderer;
    Transform tr;
    Rigidbody rb;
    int hitCount = 0;
    Collider[] colls = new Collider[10];

    void Start()
    {
        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        // 하위에 있는 component 연결
        renderer = GetComponentInChildren<MeshRenderer>();
        // 난수 발생
        int idx = Random.Range(0, textures.Length);
        // 텍스쳐 지정
        renderer.material.mainTexture = textures[idx];
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
            tr.position, Quaternion.identity, barrelEffectTr);
        // 폭발효과 5초 후에 제거
        Destroy(exp, DESTROY_EXP);
        // Barrel의 무게를 가볍게 함
        //rb.mass = BARREL_MASS;
        // 위로 솟구치는 힘을 가함
        //rb.AddForce(Vector3.up * UP_FORCE);
        // 간접 폭발력 전달
        IndirectDamage(tr.position);
        // 3초 후에 Barrel 제거
        Destroy(gameObject, DESTROY_BARREL);
    }

    void IndirectDamage(Vector3 pos)
    {
        // 주변에 있는 드럼통을 모두 추출
        // GC 발생
        //Collider[] colls = Physics.OverlapSphere(pos, radius, 1 << 3);
        // GC 없음
        Physics.OverlapSphereNonAlloc(pos, radius, colls, 1 << 3);
        foreach (var item in colls)
        {
            if (item == null) continue;
            // 폭발 범위에 포함된 드럼통의 RB
            rb = item.GetComponent<Rigidbody>();
            rb.mass = BARREL_MASS;
            rb.constraints = RigidbodyConstraints.None;
            // 폭발력을 전달
            rb.AddExplosionForce(UP_FORCE, pos, radius, OVER_FORCE);
        }
    }
}
