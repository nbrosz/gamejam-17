using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFacingBillboard : MonoBehaviour {

	void Update () {
        var cameraRotation = Camera.main.transform.rotation * Vector3.forward;
        cameraRotation.y = 0;
        transform.LookAt(transform.position + cameraRotation, Vector3.up);
	}
}
