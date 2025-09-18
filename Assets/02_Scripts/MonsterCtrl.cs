using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Navigation
using UnityEngine.AI;
public class MonsterCtrl : MonoBehaviour
{
    Transform monsterTr;
    Transform playerTr;
    NavMeshAgent agent;
    void Start()
    {
        // monsterTr
        monsterTr = GetComponent<Transform>();
        // playerTr
        playerTr = GameObject.FindWithTag("PLAYER")
            .GetComponent<Transform>();
        // agent
        agent = GetComponent<NavMeshAgent>();
        // start trace
        //agent.destination = playerTr.position;
        agent.SetDestination(playerTr.position);
    }
}
