using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class sc_quit : MonoBehaviour {
    public Button btn_Quit;

	// Use this for initialization
	void Start () {
        Button quit = btn_Quit.GetComponent<Button>();
        quit.onClick.AddListener(QuitGame);
	}
	
    //Exit the game
    void QuitGame () {
        Application.Quit();
    }

	// Update is called once per frame
	void Update () {
		
	}
}
