using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour {

    private enum EnemyFacingState { Front, SideR, Back, SideL }

    public BillboardAnimator boardAnimator;

    public float rotateSpeed;
    public float searchTime;
    public float chargeGunTime;
    public float stunTime;

    private NavMeshAgent agent;
    private Transform player;
    private bool chasing;
    private bool searching;
    private float searchTimer;
    private float chargeGunTimer;
    private float stunTimer;
    private bool stunned;
    private bool ableToShoot;
    private Health healthScript;
    private EnemyFacingState facingState;

    private Quaternion lookRotation;
    private Vector3 direction;

    private GameObject GOplayer;

    private Collider[] colliders;

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
        chargeGunTimer = 0;
        stunTimer = 0;
        healthScript = gameObject.GetComponent<Health>();
        GOplayer = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update() {
        if ( GOplayer.GetComponent<Health>().Alive ) {
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
                        if (!boardAnimator.PlayingOrUpNext(GetAnimationName("Walk"))) {
                            boardAnimator.PlayAnimation(GetAnimationName("Walk"));
                        }

                        // Charge gun up
                        if ( !ableToShoot && chargeGunTimer < chargeGunTime ) {
                            chargeGunTimer += Time.deltaTime;
                            if ( chargeGunTimer >= chargeGunTime )
                                ableToShoot = true;
                        }
                    } else {
                        // play idle animation
                    }

                    // Player has left view radius but is close by, continue following for short time
                    if ( searching ) {
                        // Timer for searching
                        searchTimer += Time.deltaTime;
                        if ( searchTimer >= searchTime ) {
                            chasing = false;
                            searching = false;
                            searchTimer = 0;
                            chargeGunTimer = 0;
                            agent.ResetPath();
                        }
                    }

                    // Shoot the player
                    if ( ableToShoot && GameController.Beat ) {
                        boardAnimator.QuickPlayAnimation("Attack"); // play attack animation
                        // send message to attack
                        SendMessage("DoAttack", Attack.AttackType.WeakAndWide, SendMessageOptions.DontRequireReceiver);
                        chargeGunTimer = 0;
                        ableToShoot = false;
                    }
                }

                // Take time to recover from stun
                else {
                    stunTimer += Time.deltaTime;
                    if ( stunTimer >= stunTime ) {
                        stunned = false;
                        stunTimer = 0;
                    }
                }
            }
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
        Debug.Log(gameObject.name + " has been hurt!");
    }

    void OnKilled() {
        foreach(var collider in colliders) {
            collider.enabled = false; // disable all colliders so enemy can be walked through
        }
        Debug.Log(gameObject.name + " has been killed!");
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
}
