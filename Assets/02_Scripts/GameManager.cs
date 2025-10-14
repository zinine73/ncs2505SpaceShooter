using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //public Transform[] points;
    public List<Transform> points = new List<Transform>();

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
        foreach(Transform point in spg)
        {
            points.Add(point);
        }
    }
}
