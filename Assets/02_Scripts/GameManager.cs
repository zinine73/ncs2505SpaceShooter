using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class GameManager : MySingleton<GameManager>
{
    //public Transform[] points;
    public List<Transform> points = new List<Transform>();
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
        Instantiate(monster, points[idx].position, points[idx].rotation);
    }
}
