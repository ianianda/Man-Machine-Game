using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    NavMeshAgent agent;
    PlayerCharacter character;
    Transform player;

    void Start()
    {
        character = GetComponent<PlayerCharacter>();
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        InvokeRepeating("FireControl", 1, 3);
    }

    void FireControl()
    {
        character.Attack();
    }

    void Update()
    {
        agent.destination = player.position; //follow the player's direction.
        transform.LookAt(player.transform);
    }

}
