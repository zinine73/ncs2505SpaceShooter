using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 반드시 필요한 component 명시
[RequireComponent(typeof(AudioSource))]
public class FireCtrl : MonoBehaviour
{
    public Transform bulletCreateTr;
    public GameObject bullet;
    public Transform firePos;
    public AudioClip fireSfx;
    new AudioSource audio;
    MeshRenderer muzzleFlash;
    bool isPlayerDie = false;
    void OnEnable()
    {
        PlayerCtrl.OnPlayerDie += OnPlayerDie;
    }
    void OnDisable()
    {
        PlayerCtrl.OnPlayerDie -= OnPlayerDie;
    }
    void Start()
    {
        audio = GetComponent<AudioSource>();
        muzzleFlash = firePos.GetComponentInChildren<MeshRenderer>();
        muzzleFlash.enabled = false;
    }

    void Update()
    {
        if (isPlayerDie) return;

        // 마우스 왼쪽 클릭했을 때 Fire함수 호출
        if (Input.GetMouseButtonDown(0))
        {
            Fire();
        }
    }

    void Fire()
    {
        // Bullet prefab을 동적으로 생성
        Instantiate(bullet, firePos.position, firePos.rotation, bulletCreateTr);
        audio.PlayOneShot(fireSfx, 1.0f);
        StartCoroutine(ShowMuzzleFlash());
    }

    IEnumerator ShowMuzzleFlash()
    {
        // 오프셋 좌표값을 랜덤함수로 생성
        Vector2 offset = new Vector2(Random.Range(0, 2), Random.Range(0, 2)) * 0.5f;
        // 텍스처의 오프셋 값 설정
        muzzleFlash.material.mainTextureOffset = offset;
        //muzzleFlash.material.SetTextureOffset("_MainTxt", offset);
        // 회전 변경
        float angle = Random.Range(0, 360);
        //muzzleFlash.transform.localRotation = Quaternion.Euler(0, 0, angle);
        muzzleFlash.transform.localRotation = Quaternion.Euler(Vector3.forward * angle);
        // 크기 조절
        float scale = Random.Range(1.0f, 2.0f);
        muzzleFlash.transform.localScale = Vector3.one * scale;

        muzzleFlash.enabled = true;
        yield return new WaitForSeconds(0.2f);
        muzzleFlash.enabled = false;
    }

    void OnPlayerDie()
    {
        isPlayerDie = true;
    }
}
