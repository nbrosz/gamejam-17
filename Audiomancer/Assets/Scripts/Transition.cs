using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition : MonoBehaviour {
    public Transform transitionState;
    public float transitionTime;

    private float currentTransitionTime = 0;
    private bool transitionUp = false;

    private Vector3 startPosition;
    private Quaternion startRotation;
    private Vector3 startScale;

    void Start() {
        startPosition = transform.position;
        startRotation = transform.rotation;
        startScale = transform.localScale;
    }

    void Update() {
        if (transitionUp && currentTransitionTime < transitionTime) {
            currentTransitionTime += Time.deltaTime;

            if (currentTransitionTime > transitionTime)
                currentTransitionTime = transitionTime;

            UpdateTransition();
        } else if (!transitionUp && currentTransitionTime > 0) {
            currentTransitionTime -= Time.deltaTime;

            if (currentTransitionTime < 0)
                currentTransitionTime = 0;

            UpdateTransition();
        }
    }

    void UpdateTransition() {
        var currentTime = currentTransitionTime / transitionTime;
        transform.position = Vector3.Lerp(startPosition, transitionState.position, currentTime);
        transform.rotation = Quaternion.Lerp(startRotation, transitionState.rotation, currentTime);
        transform.localScale = Vector3.Lerp(startScale, transitionState.localScale, currentTime);
    }

    public void DoTransition() {
        transitionUp = !transitionUp;
    }
}
