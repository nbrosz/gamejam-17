using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour {

    private NavMeshAgent agent;
    public bool test;

	// Use this for initialization
	void Start () {
        agent = GetComponent<NavMeshAgent>();
        test = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // Check if player is within view radius
    void OnTriggerStay(Collider other) {
        if (other.tag == "Player") {    // Check for player
            if (Vector3.Angle(transform.forward, other.transform.position) <= 70) {
                agent.SetDestination(other.transform.position);
            }
        }
    }
}
