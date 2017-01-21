using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Widen : MonoBehaviour {

    public float endSize;
    public float timeToWiden;

    private float startSize;
    private float currentTime = 0;

    void Start() {
        startSize = transform.localScale.x;
    }

	void Update () {
        if (currentTime < timeToWiden) {
            currentTime += Time.deltaTime;
            if (currentTime > timeToWiden)
                currentTime = timeToWiden;

            var currentScale = transform.localScale;
            currentScale.x = startSize + (endSize - startSize) * (currentTime / timeToWiden);
            transform.localScale = currentScale;
        }
	}
}
