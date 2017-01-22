using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_launchPause : MonoBehaviour {

    public GameObject btn_Cont;
    public GameObject btn_Quit;
    public GameObject btn_toMenu;

	// Use this for initialization
	void Start () {
        btn_Cont.SetActive(false);
        btn_Quit.SetActive(false);
        btn_toMenu.SetActive(false);
	}

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale == 1)
            {
                Time.timeScale = 0;
                btn_Cont.SetActive(true);
                btn_Quit.SetActive(true);
                btn_toMenu.SetActive(true);
            }
            else
            {
                Time.timeScale = 1;
                btn_Cont.SetActive(false);
                btn_Quit.SetActive(false);
                btn_toMenu.SetActive(false);
            }
        }
    }
}
