using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public static bool Beat { get { return _instance.beat; } }
    public static bool OnBeat {
        get {
            var allowedBeatOffset = _instance.BeatTime * _instance.allowedBeatOffset * .5f; // calculate allowed time and split in half
            return _instance.currentBeats >= _instance.BeatTime - allowedBeatOffset // half of allowed time too fast
                ||  _instance.currentBeats <= allowedBeatOffset; // or half of allowed time too slow
        }
    }

    private float BeatTime { get { return (60 / beatsPerMinute); } }

    private static GameController _instance;

    public AudioSource audioSource;
    public AudioClip[] beats;

    public float beatsPerMinute = 90;
    public float allowedBeatOffset = .25f;

    private float currentBeats = 0;
    private bool beat = false;

    private int beatIndex = 0;

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

        if (currentBeats < BeatTime) {
            currentBeats += Time.deltaTime;

            if (currentBeats >= BeatTime) {
                currentBeats = 0;
                beat = true;
                PlayBeat();
            }
        }
	}

    void PlayBeat() {
        audioSource.PlayOneShot(beats[beatIndex]);
        beatIndex = (beatIndex + 1) % beats.Length;
    }
}
