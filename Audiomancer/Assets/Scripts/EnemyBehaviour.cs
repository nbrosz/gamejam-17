using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour {

    public float rotateSpeed;
    public float searchTime;

    private NavMeshAgent agent;
    private Transform player;
    private bool chasing;
    private bool searching;
    private float timer;

    private Quaternion lookRotation;
    private Vector3 direction;



	// Use this for initialization
	void Start () {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player").transform;
        chasing = false;
        searching = false;
	}
	
	// Update is called once per frame
	void Update () {
        // Set destination as player as long as we're chasing it
		if (chasing) {
            agent.SetDestination(player.position);

            // Find vector pointing from current position to player
            direction = (player.position - transform.position).normalized;

            // Create the rotation towards player
            lookRotation = Quaternion.LookRotation(direction);

            // Rotate towards lookRotation over time
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotateSpeed);
        }

        // Player has left view radius but is close by, continue following for short time
        if (searching) {
            // Timer for searching
            timer += Time.deltaTime;
            if (timer >= searchTime) {
                chasing = false;
                searching = false;
                timer = 0;
                agent.ResetPath();
            }
        }
	}

    // Check if player is within view radius
    void OnTriggerStay(Collider other) {
        if (other.tag == "Player") {    // Check for player
            float angle = Vector3.Angle(transform.forward, other.transform.position - transform.position);
            if (angle <= 90) {
                chasing = true;
                searching = false;
                timer = 0;
            } else {
                if (!searching && chasing)
                    searching = true;
            }
        }
    }

    // Check if player has left the view radius
    void OnTriggerExit(Collider other) {
        if (other.tag == "Player") {
            if (!searching && chasing)
                searching = true;
        }
    }
}
