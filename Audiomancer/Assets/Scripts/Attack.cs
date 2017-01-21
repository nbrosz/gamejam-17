using System.Collections;
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
        if (attackCooldown <= 0 && attackPrototypes.Length > attackIndex) {
            attackCooldown = attackCooldownMax;

            var attack = Instantiate(attackPrototypes[attackIndex]);
            attack.transform.position = transform.position;
            attack.transform.rotation = transform.rotation;
            attack.name += string.Format("[{0}]|{1}|{{{2}}}", attackBaseDamage, string.Join(",", attackDamageTags), gameObject.name);
            SendMessage("OnAttack", attackType, SendMessageOptions.DontRequireReceiver);
        }
    }
}
