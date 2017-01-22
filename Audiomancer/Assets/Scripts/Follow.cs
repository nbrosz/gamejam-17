using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour {
    public Transform follower;

    void Start() {
        follower.parent = null;
    }

	void Update () {
        if (follower.transform.position != transform.position)
            follower.transform.position = transform.position;
	}
}
