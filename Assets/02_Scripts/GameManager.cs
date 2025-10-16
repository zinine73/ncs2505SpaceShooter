using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Mono.Cecil.Cil;
using UnityEngine;

public class GameManager : MySingleton<GameManager>
{
    //public Transform[] points;
    public List<Transform> points = new List<Transform>();
    // 몬스터를 미리 생성해 저장할 리스트
    public List<GameObject> monsterPool = new List<GameObject>();
    // 오브젝트풀에 생성할 몬스터의 최대 개수
    public int maxMonsters = 10;
    public GameObject monster;
    public float createTime = 3.0f;
    bool isGameOver;
    // Property
    public bool IsGameOver
    {
        get { return isGameOver; }
        set
        {
            isGameOver = value;
            if (isGameOver)
            {
                CancelInvoke("CreateMonster");
            }
        }
    }

    // singleton
    /*
    public static GameManager Instance = null;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }
    */
    void Start()
    {
        // 몬스터 오브젝트 풀 생성
        CreateMonsterPool();

        /*
        GameObject go = GameObject.Find("SPG");
        if (go != null)
        {
            Transform spg = go.transform;
            if (spg != null)
            {
                points = spg.GetComponentsInChildren<Transform>();
            }
        }
        */
        Transform spg = GameObject.Find("SPG")?.transform;
        //points = spg?.GetComponentsInChildren<Transform>();
        //spg?.GetComponentsInChildren<Transform>(points);
        foreach (Transform point in spg)
        {
            points.Add(point);
        }

        InvokeRepeating("CreateMonster", 2.0f, createTime);
    }

    void CreateMonster()
    {
        int idx = Random.Range(0, points.Count);
        //Instantiate(monster, points[idx].position, points[idx].rotation);
        // 오브젝트 풀에서 몬스터 추출
        GameObject _monster = GetMonsterInPool();
        // 추출한 몬스터의 위치와 회전값 설정
        _monster?.transform.SetPositionAndRotation(
            points[idx].position, points[idx].rotation
        );
        // 추출한 몬스터를 활성화
        _monster?.SetActive(true);
    }

    void CreateMonsterPool()
    {
        for (int i = 0; i < maxMonsters; i++)
        {
            // 몬스터 생성
            var _monster = Instantiate<GameObject>(monster);
            // 몬스터의 이름을 지정
            //_monster.name = "Monster" + i.ToString("00");
            _monster.name = $"Monster_{i:00}";
            // 몬스터 비활성화
            _monster.SetActive(false);
            // 생성한 몬스터를 오브젝트 풀에 추가
            monsterPool.Add(_monster);
        }
    }

    // 오브젝트 풀에서 사용 가능한 몬스터를 추출해 반환하는 함수
    public GameObject GetMonsterInPool()
    {
        // 오브젝트 풀을 순회
        foreach (var _monster in monsterPool)
        {
            // 비활성화 여부로 사용 가능한 몬스터를 판단
            if (_monster.activeSelf == false)
            {
                // 몬스터 반환
                return _monster;
            }
        }
        return null;
    }
}
