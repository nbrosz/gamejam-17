using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public static bool Beat { get { return _instance.beat; } }

    private static GameController _instance;

    public float beatsPerMinute;

    private float currentBeats = 0;
    private bool beat = false;

    void Awake() {
        // Force singleton pattern
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);
    }

	void Update () {
        if (beat)
            beat = false;

		if (currentBeats < beatsPerMinute) {
            currentBeats += Time.deltaTime;

            if (currentBeats >= beatsPerMinute) {
                currentBeats = 0;
                beat = true;
            }
        }
	}
}
