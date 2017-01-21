using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Propel : MonoBehaviour {

    public new Rigidbody rigidbody;
    public float speed;

	void Update () {
        rigidbody.AddForce(speed * (transform.rotation * Vector3.forward), ForceMode.VelocityChange);
	}
}
