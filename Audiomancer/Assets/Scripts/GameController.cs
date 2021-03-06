﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public static bool Beat { get { return Instance.beat; } }
    public static bool OnBeat {
        get {
            var allowedBeatOffset = Instance.BeatTime * Instance.allowedBeatOffset * .5f; // calculate allowed time and split in half
            return Instance.currentBeatTimer >= Instance.BeatTime - allowedBeatOffset // half of allowed time too fast
                || Instance.currentBeatTimer <= allowedBeatOffset; // or half of allowed time too slow
        }
    }
    public static int TotalBeats { get { return Instance.totalBeats; } }

    private float BeatTime { get { return (60 / beatsPerMinute); } }

    private static GameController Instance {
        get {
            if (_instance == null)
                throw new System.Exception("Require GameController in scene!");
            else
                return _instance;
        }
    }
    private static GameController _instance;

    public AudioSource audioSource;
    public AudioClip[] beats;

    public float beatsPerMinute = 90;
    public float allowedBeatOffset = .25f;

    private float currentBeatTimer = 0;
    private bool beat = false;

    private int beatIndex = 0;
    private int totalBeats = 0;

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

        if (currentBeatTimer < BeatTime) {
            currentBeatTimer += Time.deltaTime;

            if (currentBeatTimer >= BeatTime) {
                currentBeatTimer = 0;
                beat = true;
                totalBeats++;
                PlayBeat();
            }
        }
	}

    void PlayBeat() {
        audioSource.PlayOneShot(beats[beatIndex]);
        beatIndex = (beatIndex + 1) % beats.Length;
    }
}
