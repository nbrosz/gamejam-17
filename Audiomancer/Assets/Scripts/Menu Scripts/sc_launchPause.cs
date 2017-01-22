using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_launchPause : MonoBehaviour {

    public GameObject[] PauseScreenItems;

	// Use this for initialization
	void Start () {
        foreach ( GameObject g in PauseScreenItems ) {
            g.SetActive(false);
        }
	}

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale == 1)
            {
                Time.timeScale = 0;
                foreach ( GameObject g in PauseScreenItems ) {
                    g.SetActive(true);
                }
            }
            else
            {
                Time.timeScale = 1;
                foreach ( GameObject g in PauseScreenItems ) {
                    g.SetActive(false);
                }
            }
        }
    }
}
