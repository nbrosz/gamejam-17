using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTouchCallFunction : MonoBehaviour {

    public string[] tagsToAllow;
    public GameObject targetGameObject;
    public string functionToCall;

    void OnTriggerEnter(Collider collider) {
        foreach (var tag in tagsToAllow) {
            if (collider.tag.Contains(tag)) {
                TriggerFunction();
                break;
            }
        }
    }

    void TriggerFunction() {
        if (targetGameObject != null && functionToCall != null) {
            targetGameObject.SendMessage(functionToCall, SendMessageOptions.DontRequireReceiver);
        }
    }
}
