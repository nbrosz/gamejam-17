using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class DestroyOnTouch : MonoBehaviour {

    void OnTriggerEnter(Collider col) {
        var objectCreator = ParseCreator(gameObject.name);
        var collisionCreator = ParseCreator(col.gameObject.name);
        // if this object isn't hitting its creator AND the hit object doesn't have a creator or the hit object doesn't share this object's creator...
        if (col.name != objectCreator && (collisionCreator == null || objectCreator != collisionCreator)) {
            Destroy(gameObject); // destroy this object
        }
    }

    string ParseCreator(string colliderName) {
        var match = Regex.Match(colliderName, @"(?<=\{)(\D+)(?=\})");
        if (match.Success)
            return match.Value;
        else
            return null;
    }
}
