using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class sc_start : MonoBehaviour {
    public Button btn_Start;

	// Use this for initialization
	void Start () {
        Button start = btn_Start.GetComponent<Button>();
        start.onClick.AddListener(StartGame);
	}
	
    // Start Game
    void StartGame()
    {
        SceneManager.LoadScene("Level");
    }

	// Update is called once per frame
	void Update () {
		
	}
}
