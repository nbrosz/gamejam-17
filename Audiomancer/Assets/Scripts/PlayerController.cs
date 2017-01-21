using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayerController : MonoBehaviour {

    public FirstPersonController fpController;

    private bool alive = true;

    void OnDamaged() {
        Debug.Log(gameObject.name + " has been hurt!");
    }

    void OnKilled() {
        alive = false;
        fpController.canMove = false;
        Debug.Log(gameObject.name + " has been killed!");
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0)) {
            SendMessage("DoAttack", Attack.AttackType.WeakAndWide, SendMessageOptions.DontRequireReceiver);
        }
	}
}
