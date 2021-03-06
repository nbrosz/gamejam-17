﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour {

    private enum EnemyFacingState { Front, SideR, Back, SideL }

    public BillboardAnimator boardAnimator;

    public float rotateSpeed;
    public float searchTime;
    public int chargeGunBeat;
    public int MeasureCount;
    public Attack.AttackType[] attackOrder;
    public float stunTime;

    private NavMeshAgent agent;
    private Transform player;
    private bool chasing;
    private bool searching;
    private float searchTimer;
    private float stunTimer;
    private bool stunned;
    private bool ableToShoot;
    private Health healthScript;
    private EnemyFacingState facingState;

    private Quaternion lookRotation;
    private Vector3 direction;

    private GameObject GOplayer;

    private Collider[] colliders;

    private int attackOrderIndex = 0;

    void Awake() {
        colliders = gameObject.GetComponentsInChildren<Collider>(); // preserve all colliders attached to enemy
    }

	// Use this for initialization
	void Start() {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player").transform;
        ableToShoot = false;
        chasing = false;
        searching = false;
        stunned = false;
        searchTimer = 0;
        stunTimer = 0;
        healthScript = gameObject.GetComponent<Health>();
        GOplayer = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update() {
        // Make sure player is alive still
        if ( GOplayer.GetComponent<Health>().Alive ) {
            GetFacingSide();
            // Make sure enemy is able to do things
            if ( healthScript.Alive ) {
                if ( !stunned ) {
                    // Set destination as player as long as we're chasing it
                    if ( chasing ) {
                        agent.SetDestination(player.position);

                        // Find vector pointing from current position to player
                        direction = (player.position - transform.position).normalized;

                        // Create the rotation towards player
                        lookRotation = Quaternion.LookRotation(direction);

                        // Rotate towards lookRotation over time
                        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotateSpeed);

                        // Animate walking
                        if ( !boardAnimator.PlayingOrUpNext(GetAnimationName("Walk")) ) {
                            boardAnimator.PlayAnimation(GetAnimationName("Walk"));
                        }

                        // Charge gun up
                        if ( !ableToShoot && GameController.Beat ) {
                            if (GameController.TotalBeats % (MeasureCount + 1) == chargeGunBeat) {
                                ableToShoot = true;
                            }
                        }
                    } else {
                        // play idle animation
                        if ( !boardAnimator.PlayingOrUpNext(GetAnimationName("Idle")) ) {
                            boardAnimator.PlayAnimation(GetAnimationName("Idle"));
                        }
                    }

                    // Player has left view radius but is close by, continue following for short time
                    if ( searching ) {
                        // Timer for searching
                        searchTimer += Time.deltaTime;
                        if ( searchTimer >= searchTime ) {
                            chasing = false;
                            searching = false;
                            searchTimer = 0;
                            agent.ResetPath();
                        }
                    }

                    // Shoot the player
                    if ( ableToShoot && GameController.Beat ) {
                        boardAnimator.QuickPlayAnimation("Attack"); // play attack animation
                        // send message to attack
                        SendMessage("DoAttack", attackOrder[attackOrderIndex], SendMessageOptions.DontRequireReceiver);
                        attackOrderIndex = (attackOrderIndex + 1) % attackOrder.Length;
                        ableToShoot = false;
                    }
                }

                // Take time to recover from stun
                else {
                    boardAnimator.PlayAnimation(GetAnimationName("Idle"), true);
                    stunTimer += Time.deltaTime;
                    if ( stunTimer >= stunTime ) {
                        stunned = false;
                        stunTimer = 0;
                    }
                }
            } else {
                if ( !boardAnimator.PlayingOrUpNext(GetAnimationName("Dead")) ) {
                    boardAnimator.PlayAnimation(GetAnimationName("Dead"));
                }
            }
        } else {
            if ( !boardAnimator.PlayingOrUpNext(GetAnimationName("Idle")) ) {
                boardAnimator.PlayAnimation(GetAnimationName("Idle"));
            }
            agent.ResetPath();
        }
	}

    // Check if player is within view radius
    void OnTriggerStay(Collider other) {
        if (other.tag == "Player") {    // Check for player
            // Make sure enemy is active
            if ( !stunned && healthScript.Alive ) {
                float angle = Vector3.Angle(transform.forward, other.transform.position - transform.position);
                if ( angle <= 90 ) {
                    chasing = true;
                    searching = false;
                    searchTimer = 0;
                } else {
                    if ( !searching && chasing )
                        searching = true;
                }
            }
        }
    }

    // Check if player has left the view radius
    void OnTriggerExit(Collider other) {
        if ( other.tag == "Player" ) {
            // Make sure enemy is active and saw player recently
            if ( !searching && chasing && !stunned && healthScript.Alive )
                searching = true;
        }
    }

    void OnParried(AttackShot.ParriedData parriedData) {
        if ( parriedData.attacker.owner == gameObject ) {
            stunned = true;
            agent.ResetPath();
            Debug.Log(gameObject.name + " is stunned from being parried!"); // replace
        }
    }

    void OnDamaged() {
        // Start chasing player when shot, even if player is not seen
        chasing = true;
        searching = true;
        //Debug.Log(gameObject.name + " has been hurt!");
    }

    void OnKilled() {
        foreach(var collider in colliders) {
            collider.enabled = false; // disable all colliders so enemy can be walked through
        }
        //Debug.Log(gameObject.name + " has been killed!");
        agent.ResetPath();
        boardAnimator.PlayAnimation("FrontDeath", true);
        boardAnimator.QueueAnimation("FrontDead");
    }

    string GetAnimationName(string animationType) {
        switch (facingState) {
            case EnemyFacingState.Front: return "Front" + animationType;
            case EnemyFacingState.Back: return "Back" + animationType;
            case EnemyFacingState.SideR: return "Side" + animationType;
            case EnemyFacingState.SideL: return "Side" + animationType;
        }
        return null;
    }

    void SetLeftFlip(bool isFacingLeft) {
        var scale = boardAnimator.transform.localScale;
        if (!isFacingLeft) {
            scale.x = Mathf.Abs(scale.x);
        } else {
            scale.x = -Mathf.Abs(scale.x);
        }

        boardAnimator.transform.localScale = scale;
    }

    void GetFacingSide() {
        bool isFacingLeft = false;
        float angle = Vector3.Angle(transform.forward, GOplayer.transform.position - transform.position);

        if (angle <= 45) {  // Front facing
            facingState = EnemyFacingState.Front;

        } else if (angle >= 135) {  // Back Facing
            facingState = EnemyFacingState.Back;

        } else { // Facing a side
            Vector3 target = GOplayer.transform.position - transform.position;
            Vector3 perp = Vector3.Cross(transform.forward, target);
            float dir = Vector3.Dot(perp, transform.up);

            if (dir < 0) {  // Left Facing
                isFacingLeft = true;
                facingState = EnemyFacingState.SideL;

            } else {    // Right Facing
                facingState = EnemyFacingState.SideR;
            }
        }
        SetLeftFlip(isFacingLeft);
    }
}
