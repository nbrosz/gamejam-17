﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayerController : MonoBehaviour {

    public FirstPersonController fpController;
    public Health health;

    void OnDamaged() {
        Debug.Log(gameObject.name + " has been hurt!");
    }

    void OnKilled() {
        fpController.canMove = false;
        Debug.Log(gameObject.name + " has been killed!");
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (health.Alive && Time.timeScale > 0 && Input.GetMouseButtonDown(0)) {
            SendMessage("DoAttack", Attack.AttackType.WeakAndWide, SendMessageOptions.DontRequireReceiver);
        }
	}
}
