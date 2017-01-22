using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class AttackShot : MonoBehaviour {

    public Attack.AttackType type;
    [HideInInspector]
    public GameObject owner;
    [HideInInspector]
    public float damage;
    [HideInInspector]
    public string[] damageTags;

    void OnTriggerEnter(Collider col) {
        var otherAttackShot = col.GetComponent<AttackShot>();

        if (!otherAttackShot) {
            if (col.isTrigger == false && !InheritsFromGameObject(col.gameObject, owner) && col.tag != "Ignore") {
                if (damage != 0 && AttackDoesDamage(damageTags, col.gameObject)) { // positive damage value indicates single-shot damage
                    col.gameObject.SendMessage("DoDamage", damage, SendMessageOptions.DontRequireReceiver);
                } else {
                    Destroy(gameObject);
                }
            }
        } else {
            if (otherAttackShot.owner != owner && otherAttackShot.type == type) {
                otherAttackShot.owner.gameObject.SendMessage("OnParried", new ParriedData(otherAttackShot, this), SendMessageOptions.DontRequireReceiver);
                //Destroy(gameObject);
            }
        }
    }

    bool InheritsFromGameObject(GameObject go, GameObject inheritsFrom) {
        var currentTransform = go.transform;
        while (currentTransform != null && currentTransform.gameObject != inheritsFrom) {
            currentTransform = currentTransform.parent;
        }

        return currentTransform != null && currentTransform.gameObject == inheritsFrom;
    }

    bool AttackDoesDamage(string[] damageTags, GameObject target) {
        if (damageTags == null || damageTags.Length == 0)
            return true;

        foreach (var tag in damageTags) {
            if (target.tag.Contains(tag)) // see if gameObject's tag matches any of the damage tags
                return true;
        }

        return false;
    }

    public class ParriedData {
        public AttackShot attacker;
        public AttackShot parrier;
        public ParriedData(AttackShot attacker, AttackShot parrier) {
            this.attacker = attacker;
            this.parrier = parrier;
        }
    }

}
