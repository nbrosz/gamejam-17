﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour {

    public enum AttackType { WeakAndWide = 0, Medium = 1, StrongAndNarrow = 2}
    public string[] attackDamageTags;
    public GameObject[] attackPrototypes;
    public float[] attackBaseDamage;
    public float attackCooldownMax = 1f;

    private float attackCooldown = 0;

    void Update() {
        if (attackCooldown > 0) {
            attackCooldown -= Time.deltaTime;
        }
    }

    void DoAttack(AttackType attackType) {
        var attackIndex = (int)attackType;
        if (attackCooldown <= 0 && attackPrototypes.Length > attackIndex && Time.timeScale > 0) {
            attackCooldown = attackCooldownMax;

            var attack = Instantiate(attackPrototypes[attackIndex]);
            attack.transform.position = transform.position;
            attack.transform.rotation = transform.rotation;

            var attackShot = attack.GetComponent<AttackShot>();
            attackShot.owner = gameObject;
            attackShot.type = attackType;
            attackShot.damage = attackBaseDamage[attackIndex];
            attackShot.damageTags = attackDamageTags;

            SendMessage("OnAttack", attackType, SendMessageOptions.DontRequireReceiver);
        }
    }
}
