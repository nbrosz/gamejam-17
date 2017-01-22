using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomLocation : MonoBehaviour {

    public Transform[] randomTransforms;

    void Awake() {
        if (randomTransforms.Length > 0) {
            // pick a random transform and set this character's position/rotation to match
            var randomTransform = randomTransforms[Random.Range(0, randomTransforms.Length)];
            transform.position = randomTransform.position;
            transform.rotation = randomTransform.rotation;
        }
    }
}
