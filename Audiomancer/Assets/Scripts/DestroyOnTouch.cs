using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class DestroyOnTouch : MonoBehaviour {

    void OnTriggerEnter(Collider col) {
        if (!HitCreator(col.gameObject.name) && col.gameObject.name != gameObject.name) {
            Destroy(gameObject);
        }
        
    }

    bool HitCreator(string colliderName) {
        var match = Regex.Match(gameObject.name, @"(?<=\{)(\D+)(?=\})");
        if (match.Success) {
            if (colliderName == match.Value) {
                return true;
            }
        }

        return false;
    }
}
